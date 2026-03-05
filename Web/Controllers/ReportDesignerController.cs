using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ReportDesignerController : Controller
    {
        // id parametresi şablonun veritabanındaki ID'sidir. 
        // Eğer URL'den bir şey gelmezse "Yeni_Tasarim" olarak kabul ederiz.
        public IActionResult Index(string id = "Yeni_Tasarim")
        {
            // Bu ID'yi View'a (oradan da DevExpress'in motoruna) gönderiyoruz.
            ViewBag.ReportId = id;
            return View();
        }
    }
}