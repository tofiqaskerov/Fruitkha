using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        [Area("dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
