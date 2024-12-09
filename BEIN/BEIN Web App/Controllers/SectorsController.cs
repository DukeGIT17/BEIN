using Microsoft.AspNetCore.Mvc;

namespace BEIN_Web_App.Controllers
{
    public class SectorsController : Controller
    {
        [HttpGet]
        public IActionResult Sector()
        {
            return View();
        }
    }
}
