using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevExpress.XtraReports.UI;
using Core.Repositories;
using Core.Entities;
using System.IO;

namespace Web.Controllers
{
    public class DocumentViewerController : Controller
    {
        private readonly IGenericRepository<Teklif> _teklifRepository;
        private readonly IGenericRepository<ReportTemplate> _templateRepository;

        public DocumentViewerController(
            IGenericRepository<Teklif> teklifRepository,
            IGenericRepository<ReportTemplate> templateRepository)
        {
            _teklifRepository = teklifRepository;
            _templateRepository = templateRepository;
        }

        [HttpGet]
        public async Task<IActionResult> TeklifYazdir(int id, int? templateId)
        {
            // 1. Teklif verisini çekiyoruz
            var teklif = await _teklifRepository.Where(x => x.Id == id)
                .Include(x => x.Musteri)
                .Include(x => x.Kalemler)
                    .ThenInclude(k => k.Urun)
                .FirstOrDefaultAsync();

            if (teklif == null)
            {
                return NotFound("Yazdırılmak istenen teklif bulunamadı!");
            }

            // 2. ENTITY -> DTO DÖNÜŞÜMÜ (Tasarımcıdaki temiz veri için)
            var teklifDto = new Core.DTOs.Reporting.TeklifRaporDTO
            {
                TeklifNo = teklif.TeklifNo,
                Tarih = teklif.Tarih,
                ToplamTutar = teklif.ToplamTutar,
                MusteriUnvan = teklif.Musteri?.Unvan,
                MusteriVergiNo = teklif.Musteri?.VergiNo,
                MusteriVergiDairesi = teklif.Musteri?.VergiDairesi,

                Kalemler = teklif.Kalemler.Select(k => new Core.DTOs.Reporting.TeklifKalemRaporDTO
                {
                    UrunAdi = k.Urun?.UrunAdi,
                    Miktar = k.Miktar,
                    BirimFiyat = k.BirimFiyat,
                    ToplamFiyat = k.Miktar * k.BirimFiyat
                }).ToList()
            };

            // 3. ŞABLONU ÇEKİYORUZ
            ReportTemplate template = null;

            if (templateId.HasValue)
            {
                // Kullanıcı menüden özel bir şablon seçtiyse
                template = await _templateRepository.Where(x => x.Id == templateId.Value).FirstOrDefaultAsync();
            }
            else
            {
                // Seçim yoksa en son ekleneni getir
                template = await _templateRepository
                    .Where(x => x.DocumentType == Core.Enums.DocumentType.Teklif)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();
            }

            // 4. RAPORU OLUŞTUR VE BAĞLA
            XtraReport report = new XtraReport();

            if (template != null && template.LayoutData != null)
            {
                using (var ms = new MemoryStream(template.LayoutData))
                {
                    report.LoadLayoutFromXml(ms);
                }
            }

            report.DataSource = new List<Core.DTOs.Reporting.TeklifRaporDTO> { teklifDto };

            return View(report);
        }
    }
}