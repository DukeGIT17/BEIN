using BEIN_DL.Models;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_API.Controllers
{
    [Route("api.bein.com/[controller]")]
    [ApiController]
    public class AdminFunctionsController(IAdminFunctionsService adminFunctionsService) : ControllerBase
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(AddSector))]
        public IActionResult AddSector(Sector sector)
        {
            try
            {
                _returnDictionary = adminFunctionsService.AddSectorAsync(sector).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(AddSoftwareProduct))]
        public IActionResult AddSoftwareProduct(SoftwareProduct software)
        {
            try
            {
                _returnDictionary = adminFunctionsService.AddSoftwareProduct(software).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
