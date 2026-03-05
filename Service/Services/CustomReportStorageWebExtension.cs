using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.DataAccess.ObjectBinding; // VERİ KAYNAĞI İÇİN GEREKLİ KÜTÜPHANE
using Core.Entities;
using Core.Enums;
using Core.Repositories;
using Core.UnitOfWorks;

namespace Service.Services
{
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly IGenericRepository<ReportTemplate> _templateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomReportStorageWebExtension(
            IGenericRepository<ReportTemplate> templateRepository,
            IUnitOfWork unitOfWork)
        {
            _templateRepository = templateRepository;
            _unitOfWork = unitOfWork;
        }

        public override bool CanSetData(string url) => true;
        public override bool IsValidUrl(string url) => true;

        public override byte[] GetData(string url)
        {
            try
            {
                // 1. Veritabanında şablon var mı diye bak (Örn: id = "5")
                if (int.TryParse(url, out int templateId))
                {
                    var template = _templateRepository.Where(x => x.Id == templateId).FirstOrDefault();
                    if (template != null && template.LayoutData != null)
                    {
                        return template.LayoutData;
                    }
                }

                // 2. BULUNAMAZSA: Boş rapor oluştur ama içine TEKLİF şemasını (ObjectDataSource) bağla!
                XtraReport report = new XtraReport();

                // DevExpress'e Teklif modelimizin iskeletini öğretiyoruz
                ObjectDataSource dataSource = new ObjectDataSource();
                dataSource.Name = "TeklifVeriKaynagi";
                dataSource.DataSource = typeof(Core.DTOs.Reporting.TeklifRaporDTO);

                // Veri kaynağını rapora ekle
                report.DataSource = dataSource;

                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Rapor yüklenirken hata oluştu: " + ex.Message);
            }
        }

        public override void SetData(XtraReport report, string url)
        {
            if (int.TryParse(url, out int templateId))
            {
                var template = _templateRepository.Where(x => x.Id == templateId).FirstOrDefault();
                if (template != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        report.SaveLayoutToXml(ms);
                        template.LayoutData = ms.ToArray();

                        _templateRepository.Update(template);
                        _unitOfWork.Commit();
                    }
                }
            }
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // defaultUrl bize Controller'dan gelir (Örn: "Yeni_Tasarim_Teklif" veya "Yeni_Tasarim_Siparis")
            // Buna göre şablonun tipini belirliyoruz. Şimdilik Teklif listesinden geldiği için Teklif yapıyoruz.
            DocumentType docType = DocumentType.Teklif;

            var newTemplate = new ReportTemplate
            {
                Name = "Teklif Şablonu - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                DocumentType = docType,
                IsDefault = true
            };

            using (var ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                newTemplate.LayoutData = ms.ToArray();
            }

            _templateRepository.AddAsync(newTemplate).Wait();
            _unitOfWork.Commit();

            return newTemplate.Id.ToString();
        }

        public override Dictionary<string, string> GetUrls()
        {
            return _templateRepository.GetAll()
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToDictionary(x => x.Id.ToString(), x => x.Name);
        }
    }
}