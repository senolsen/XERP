using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;

namespace WebUI.Controllers
{
    // 1. Rapor Görüntüleyici (Viewer) için zorunlu Controller
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService)
            : base(controllerService)
        {
        }
    }

    // 2. Rapor Tasarımcısı (Designer) için zorunlu Controller
    // DİKKAT: : işaretinden sonrasına uzun uzun DevExpress.AspNetCore... yazıyoruz.
    public class CustomReportDesignerController : DevExpress.AspNetCore.Reporting.ReportDesigner.ReportDesignerController
    {
        public CustomReportDesignerController(DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services.IReportDesignerMvcControllerService controllerService)
            : base(controllerService)
        {
        }
    }

    // 3. Veri Tabanı Sorgu Oluşturucu (Query Builder) için zorunlu Controller
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService)
            : base(controllerService)
        {
        }
    }
}