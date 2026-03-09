using Microsoft.AspNetCore.Mvc;
using DevExpress.DataAccess.ObjectBinding;
using Core.DTOs.Reporting;
using Core.Enums;
using Data.Context; // ApplicationDbContext'i görebilmek için

namespace WebUI.Controllers
{
    public class ReportDesignerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportDesignerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string id = "Yeni_Tasarim", DocumentType? docType = null)
        {
            ViewBag.ReportId = id;

            DocumentType currentDocType = DocumentType.Teklif; // Varsayılan değer

            // Eğer var olan bir şablon düzenleniyorsa, DB'den tipini bulalım
            if (id != "Yeni_Tasarim" && int.TryParse(id, out int templateId))
            {
                var template = _context.Set<Core.Entities.ReportTemplate>().Find(templateId);
                if (template != null)
                {
                    currentDocType = template.DocumentType;
                }
            }
            // Yeni tasarım ise URL'den gelen (docType=Teklif) tipini alalım
            else if (docType.HasValue)
            {
                currentDocType = docType.Value;
            }

            // Tasarımcıya gönderilecek Veri Kaynakları (DataSources)
            var dataSources = new Dictionary<string, object>();
            var objectDataSource = new ObjectDataSource();

            // DocumentType'a Göre Veri Kaynağını Dinamik Ayarlıyoruz
            switch (currentDocType)
            {
                case DocumentType.Teklif:
                    objectDataSource.Name = "TeklifVeriKaynagi";
                    objectDataSource.DataSource = typeof(TeklifRaporDTO);
                    dataSources.Add("Teklif Verileri", objectDataSource);
                    break;

                    // İleride Sipariş, Fatura vb. eklerseniz buraya "case" yazarak devam edeceksiniz.
            }

            // !! ÖNEMLİ KISIM: Veri kaynaklarını (dataSources) Dictionary model olarak View'a gönderiyoruz !!
            return View(dataSources);
        }
    }
}