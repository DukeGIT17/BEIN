using Microsoft.AspNetCore.Mvc;
using BEIN_Web_App.IClientSideServices;
using BEIN_DL.Models;

namespace BEIN_Web_App.Controllers
{
    public class GeneralController(IRequestService requestService) : Controller
    {
        private Dictionary<string, object> _returnDictionary = [];

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
