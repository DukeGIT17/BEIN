using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_API.Controllers
{
    [Route("api.bein.com/[controller]")]
    [ApiController]
    public class SectorController(ISectorService sectorService) : ControllerBase
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpGet(nameof(GetAll))]
        public IActionResult GetAll()
        {
            try                                                      
            {
                _returnDictionary = sectorService.GetAllAsync().Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetSector))]
        public IActionResult GetSector(string sectorName)
        {
            try
            {
                _returnDictionary = sectorService.GetSectorAsync(sectorName).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
