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
            var teklif = await _teklifRepository.Where(x => x.Id == id)
                .Include(x => x.Musteri)
                .Include(x => x.Kalemler)
                    .ThenInclude(k => k.Urun)
                .FirstOrDefaultAsync();

            if (teklif == null)
            {
                return NotFound("Yazdırılmak istenen teklif bulunamadı!");
            }

            var teklifDto = new Core.DTOs.Reporting.TeklifRaporDTO
            {
                TeklifNo = teklif.TeklifNo,
                Tarih = teklif.Tarih,
                BaslangicTarihi = teklif.BaslangicTarihi,
                BitisTarihi = teklif.BitisTarihi,
                ParaBirimi = teklif.ParaBirimi,
                Aciklama = teklif.Aciklama,

                AraToplam = teklif.AraToplam,
                ToplamIndirim = teklif.ToplamIndirim,
                ToplamKdv = teklif.ToplamKdv,
                ToplamTutar = teklif.ToplamTutar,

                MusteriUnvan = teklif.Musteri?.Unvan,
                MusteriVergiNo = teklif.Musteri?.VergiNo,
                MusteriVergiDairesi = teklif.Musteri?.VergiDairesi,

                Kalemler = teklif.Kalemler.Select(k => new Core.DTOs.Reporting.TeklifKalemRaporDTO
                {
                    UrunAdi = k.Urun?.UrunAdi,
                    Miktar = k.Miktar,
                    BirimFiyat = k.BirimFiyat,
                    IndirimOrani = k.IndirimOrani,
                    IndirimTutari = k.IndirimTutari,
                    KdvOrani = k.KdvOrani,
                    KdvTutari = k.KdvTutari,
                    ToplamFiyat = k.ToplamTutar
                }).ToList()
            };

            ReportTemplate template = null;

            if (templateId.HasValue)
            {
                template = await _templateRepository
                    .Where(x => x.Id == templateId.Value)
                    .FirstOrDefaultAsync();
            }
            else
            {
                template = await _templateRepository
                    .Where(x => x.DocumentType == Core.Enums.DocumentType.Teklif)
                    .OrderByDescending(x => x.IsDefault)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefaultAsync();
            }

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