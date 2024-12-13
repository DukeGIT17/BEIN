using BEIN_DL.Models;
using BEIN_Web_App.IClientSideServices;
using Microsoft.AspNetCore.Mvc;
using static System.Console;

namespace BEIN_Web_App.Controllers
{
    public class SectorsController(IRequestService requestService) : Controller
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpGet]
        public IActionResult GetSector(string sectorName)
        {
            try
            {
                _returnDictionary = requestService.GetRequestAsync<Sector>("/Sector/GetSector?sectorName=" + sectorName).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Sector sector) throw new("Could not acquire sector from data source.");
                return View("Sector", sector);
            }
            catch (Exception ex)
            {
                WriteLine($"\n\n{ex.Message.ToUpper()}\n\n");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
