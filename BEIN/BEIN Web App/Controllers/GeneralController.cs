using Microsoft.AspNetCore.Mvc;

namespace BEIN_Web_App.Controllers
{
    public class GeneralController : Controller
    {

        [HttpGet]
        public IActionResult LandingPage()
        {
            return View("Landing_Page");
        }

        [HttpGet]
        public IActionResult AboutUsPage()
        {
            return View("About_Us");
        }

        [HttpGet]
        public IActionResult ContactUsPage()
        {
            return View("Contact_Us");
        }
    }
}
