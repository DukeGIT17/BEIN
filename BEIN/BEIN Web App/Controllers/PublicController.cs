using BEIN_DL.Models;
using Microsoft.AspNetCore.Mvc;
using BEIN_Web_App.IClientSideServices;

namespace BEIN_Web_App.Controllers
{
    public class PublicController(IRequestService requestService, ILogger<PublicController> logger) : Controller
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

        [HttpGet]
        public IActionResult GetSector(string sectorName)
        {
            try
            {
                _returnDictionary = requestService.GetRequestAsync<Sector>("/Public/GetSector?sectorName=" + sectorName).Result;

                if (!(bool)_returnDictionary["Success"]) 
                    throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not Sector sector) 
                    throw new("Could not acquire sector from data source.");

                return View("Sector", sector);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult GetProduct(string productName)
        {
            try
            {
                _returnDictionary = requestService.GetRequestAsync<SoftwareProduct>("/Public/GetSoftware?softwareName=" + productName).Result;

                 if (!(bool)_returnDictionary["Success"])
                    throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not SoftwareProduct product) 
                    throw new("Could not acquire product from data source.");

                return View("Product", product);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
