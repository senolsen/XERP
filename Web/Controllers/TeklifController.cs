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
        private readonly IGenericRepository<ReportTemplate> _templateRepository;
        private readonly IDovizService _dovizService;
        private readonly IGenericRepository<ParaBirimi> _paraBirimiRepository;

        public TeklifController(
            ITeklifService teklifService,
            IGenericRepository<Teklif> teklifRepository,
            IGenericRepository<Musteri> musteriRepository,
            IGenericRepository<Urun> urunRepository,
            IGenericRepository<ReportTemplate> templateRepository,
            IDovizService dovizService,
            IGenericRepository<ParaBirimi> paraBirimiRepository)
        {
            _teklifService = teklifService;
            _teklifRepository = teklifRepository;
            _musteriRepository = musteriRepository;
            _urunRepository = urunRepository;
            _templateRepository = templateRepository;
            _dovizService = dovizService;
            _paraBirimiRepository = paraBirimiRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teklifler = await _teklifRepository.GetAll()
                .Include(x => x.Musteri)
                .Where(x => x.IsActive && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            ViewBag.Templates = await _templateRepository
                .Where(x => x.DocumentType == Core.Enums.DocumentType.Teklif && x.IsActive && !x.IsDeleted)
                .ToListAsync();

            return View(teklifler);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // DİKKAT: Tablo boşsa Merkez Bankasından tüm kurları çeker (1 Kez Çalışır)
            await _dovizService.ParaBirimleriniTcmbdenSenkronizeEtAsync();

            ViewBag.TeklifNo = await _teklifService.GetNextTeklifNoAsync();

            // Sadece Aktif olarak ayarlanmış para birimlerini arayüze gönder
            ViewBag.ParaBirimleri = await _paraBirimiRepository
                .Where(x => x.IsActive && !x.IsDeleted)
                .OrderByDescending(x => x.IsDefault)
                .ToListAsync();

            ViewBag.Kurlar = await _dovizService.GetGunlukKurlarAsync();

            return View();
        }

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
                kod = x.UrunKodu,
                fiyat = x.GuncelFiyat
            }).ToListAsync();

            return Json(new { results = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] TeklifCreateViewModel model)
        {
            if (model.MusteriId == 0 || model.Kalemler == null || !model.Kalemler.Any())
            {
                return BadRequest("Müşteri ve en az bir ürün seçmelisiniz.");
            }

            var kalemler = model.Kalemler.Select(k => new TeklifKalem
            {
                SiraNo = k.SiraNo,
                UrunKodu = k.UrunKodu,
                Aciklama = k.Aciklama,
                UrunId = k.UrunId,
                Miktar = k.Miktar,
                BirimFiyat = k.BirimFiyat,
                ParaBirimi = k.ParaBirimi,
                Kur = k.Kur,
                IndirimOrani = k.IndirimOrani,
                KdvOrani = k.KdvOrani
            }).ToList();

            var sonuc = await _teklifService.TeklifOlusturAsync(
                model.MusteriId, model.Aciklama, model.BaslangicTarihi, model.BitisTarihi, model.ParaBirimi, model.Kur, kalemler);

            return Ok(new { message = "Teklif başarıyla oluşturuldu!", teklifId = sonuc.Id });
        }
    }
}