using BEIN_DL.Models;
using BEIN_Web_App.ClientSideServices;
using BEIN_Web_App.IClientSideServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
                return View(new SoftwareProduct());
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

                var file = HttpContext.Request.Form.Files["image"];
                if (file is null)
                {
                    ModelState.AddModelError("", "Please provide a logo for this software product.");
                    _returnDictionary = requestService.GetRequestAsync<List<Sector>>("/Sector/GetAll").Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    if (_returnDictionary["Result"] is not List<Sector> sectors) throw new("Failed to acquire sectors from the API.");
                    ViewBag.sectors = sectors;
                    return View(software);
                }

                software.ImageFile = file;

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

                software.Features = [];
                var featureNames = HttpContext.Request.Form["feature_name"]!.ToString()
                    .Split(',', StringSplitOptions.TrimEntries)
                    .ToList();

                var featureDescs = HttpContext.Request.Form["feature_desc"]!.ToString()
                    .Split(',', StringSplitOptions.TrimEntries)
                    .ToList();

                for (int i = 0; i < featureNames.Count; i++)
                {
                    software.Features.Add(new()
                    {
                        Title = featureNames[i],
                        Description = featureDescs[i]
                    });
                }

                _returnDictionary = requestService.SendFileAsync(software.ImageFile!, HttpMethod.Post, $"/Files/SaveFile?name={software.Name}").Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    ModelState.AddModelError("", _returnDictionary["ErrorMessage"].ToString()!);
                    _returnDictionary = requestService.GetRequestAsync<List<Sector>>("/Sector/GetAll").Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    if (_returnDictionary["Result"] is not List<Sector> sectors) throw new("Failed to acquire sectors from the API.");
                    ViewBag.sectors = sectors;
                    return View(software);
                }

                software.ImageName = _returnDictionary["FileName"] as string;
                _returnDictionary = requestService.SendRequestAsync(software, HttpMethod.Post, "/AdminFunctions/AddSoftwareProduct").Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    ModelState.AddModelError("", _returnDictionary["ErrorMessage"].ToString()!);
                    _returnDictionary = requestService.GetRequestAsync<List<Sector>>("/Sector/GetAll").Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    if (_returnDictionary["Result"] is not List<Sector> sectors) throw new("Failed to acquire sectors from the API.");
                    ViewBag.sectors = sectors;
                    return View(software);
                }

                return RedirectToAction("LandingPage", "General");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
