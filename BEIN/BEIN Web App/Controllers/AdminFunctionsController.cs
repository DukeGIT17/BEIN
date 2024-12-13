using BEIN_DL.Models;
using BEIN_Web_App.IClientSideServices;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_Web_App.Controllers
{
    public class AdminFunctionsController(IRequestService requestService, ILogger<AdminFunctionsController> logger) : Controller
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpGet]
        public IActionResult AddSector()
        {
            return View(new Sector());
        }

        [HttpPost]
        public IActionResult AddSector(Sector sector)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(sector);


                _returnDictionary = requestService.SendRequestAsync(sector, HttpMethod.Post, "/AdminFunctions/AddSector").Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    ModelState.AddModelError("", _returnDictionary["ErrorMessage"].ToString()!);
                    return View(sector);
                }

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult AddSoftware()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSoftware(SoftwareProduct software)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(software);

                software.Sectors =
                [
                    new()
                    {
                        SectorTitle = HttpContext.Request.Form["sector"]!.ToString()
                    }
                ];

                _returnDictionary = requestService.SendRequestAsync(software, HttpMethod.Post, "/AdminFunctions/AddSoftwareProduct").Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    ModelState.AddModelError("", _returnDictionary["ErrorMessage"].ToString()!);
                    return View(software);
                }

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
