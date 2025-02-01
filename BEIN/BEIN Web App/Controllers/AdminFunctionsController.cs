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
            try
            {
                _returnDictionary = requestService.GetRequestAsync<List<Sector>>("/Sector/GetAll").Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not List<Sector> sectors) throw new("Failed to acquire sectors from the API.");
                ViewBag.sectors = sectors;
                return View();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult AddSoftware(SoftwareProduct software)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(software);

                software.Sectors = [];
                HttpContext.Request.Form["sector"]!.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList()
                    .ForEach(t => software.Sectors.Add(new()
                        {
                            SectorTitle = t,
                            ProductName = software.Name
                        }
                    ));

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
