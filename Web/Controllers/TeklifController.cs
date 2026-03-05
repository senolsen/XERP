using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TeklifController : Controller
    {
        private readonly ITeklifService _teklifService;
        private readonly IGenericRepository<Teklif> _teklifRepository;
        private readonly IGenericRepository<Musteri> _musteriRepository;
        private readonly IGenericRepository<Urun> _urunRepository;

        // YENİ EKLENEN: Açılır menüye şablonları gönderebilmek için ReportTemplate repository'si
        private readonly IGenericRepository<ReportTemplate> _templateRepository;

        // Constructor'a templateRepository eklendi
        public TeklifController(
            ITeklifService teklifService,
            IGenericRepository<Teklif> teklifRepository,
            IGenericRepository<Musteri> musteriRepository,
            IGenericRepository<Urun> urunRepository,
            IGenericRepository<ReportTemplate> templateRepository)
        {
            _teklifService = teklifService;
            _teklifRepository = teklifRepository;
            _musteriRepository = musteriRepository;
            _urunRepository = urunRepository;
            _templateRepository = templateRepository; // ATAMASI YAPILDI
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Teklifleri Çekiyoruz
            var teklifler = await _teklifRepository.GetAll()
                .Include(x => x.Musteri)
                .Where(x => x.IsActive && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            // 2. DİKKAT: Veritabanındaki aktif Teklif şablonlarını çekip ViewBag'e atıyoruz (Açılır menü için)
            ViewBag.Templates = await _templateRepository
                .Where(x => x.DocumentType == Core.Enums.DocumentType.Teklif && x.IsActive && !x.IsDeleted)
                .ToListAsync();

            return View(teklifler);
        }

        // 1. ARAYÜZÜ GETİR
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TeklifNo = "TKL-" + DateTime.Now.ToString("yyyyMMdd-HHmm");
            return View();
        }

        // 2. MÜŞTERİ ARAMA (Select2 AJAX için)
        [HttpGet]
        public async Task<IActionResult> SearchMusteri(string q)
        {
            var query = _musteriRepository.Where(x => x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Unvan.Contains(q) || x.VergiNo.Contains(q));
            }

            var data = await query.Take(20).Select(x => new {
                id = x.Id,
                text = x.Unvan
            }).ToListAsync();

            return Json(new { results = data });
        }

        // 3. ÜRÜN ARAMA (Select2 AJAX için)
        [HttpGet]
        public async Task<IActionResult> SearchUrun(string q)
        {
            var query = _urunRepository.Where(x => x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.UrunAdi.Contains(q) || x.UrunKodu.Contains(q));
            }

            var data = await query.Take(20).Select(x => new {
                id = x.Id,
                text = x.UrunAdi + " (" + x.UrunKodu + ")",
                fiyat = x.GuncelFiyat
            }).ToListAsync();

            return Json(new { results = data });
        }

        // 4. TEKLİFİ KAYDET (AJAX Post ile)
        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] TeklifCreateViewModel model)
        {
            if (model.MusteriId == 0 || model.Kalemler == null || !model.Kalemler.Any())
            {
                return BadRequest("Müşteri ve en az bir ürün seçmelisiniz.");
            }

            var kalemler = model.Kalemler.Select(k => new TeklifKalem
            {
                UrunId = k.UrunId,
                Miktar = k.Miktar,
                BirimFiyat = k.BirimFiyat
            }).ToList();

            var sonuc = await _teklifService.TeklifOlusturAsync(model.MusteriId, model.Aciklama, kalemler);

            return Ok(new { message = "Teklif başarıyla oluşturuldu!", teklifId = sonuc.Id });
        }
    }
}