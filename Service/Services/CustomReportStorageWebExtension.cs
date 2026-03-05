using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Core.Entities;
using Core.Enums;
using Core.Repositories;
using Core.UnitOfWorks;

namespace WebUI.Services
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
                if (int.TryParse(url, out int templateId))
                {
                    var template = _templateRepository.Where(x => x.Id == templateId).FirstOrDefault();
                    if (template != null && template.LayoutData != null)
                    {
                        return template.LayoutData;
                    }
                }

                using (var ms = new MemoryStream())
                {
                    new XtraReport().SaveLayoutToXml(ms);
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
            var newTemplate = new ReportTemplate
            {
                Name = "Yeni Teklif Tasarımı - " + DateTime.Now.ToString("HH:mm"),
                DocumentType = DocumentType.Teklif,
                IsDefault = false
            };

            using (var ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                newTemplate.LayoutData = ms.ToArray();
            }

            // Senkron çalışması için Wait() kullanıyoruz (DevExpress altyapısı senkron bekler)
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