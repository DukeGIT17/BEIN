using BEIN_DL.Models;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_API.Controllers
{
    [Route("api.bein.com/[controller]")]
    [ApiController]
    public class PublicController(IPublicService publicService) : ControllerBase
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpGet(nameof(GetAllSectors))]
        public IActionResult GetAllSectors()
        {
            try                                                      
            {
                _returnDictionary = publicService.GetAllSectorsAsync().Result;
                
                if (!(bool)_returnDictionary["Success"])
                    return BadRequest(_returnDictionary["ErrorMessage"]);

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
                _returnDictionary = publicService.GetSectorAsync(sectorName).Result;

                if (!(bool)_returnDictionary["Success"])
                    return BadRequest(_returnDictionary["ErrorMessage"]);

                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAllSoftware))]
        public IActionResult GetAllSoftware()
        {
            try
            {
                _returnDictionary = publicService.GetAllSoftwareProductsAsync().Result;

                if (!(bool)_returnDictionary["Success"]) 
                    return BadRequest(_returnDictionary["ErrorMessage"]);

                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetSoftware))]
        public IActionResult GetSoftware(string softwareName)
        {
            try
            {
                _returnDictionary = publicService.GetSoftwareProductAsync(softwareName).Result;

                if (!(bool)_returnDictionary["Success"]) 
                    return BadRequest(_returnDictionary["ErrorMessage"]);

                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(SearchData))]
        public IActionResult SearchData()
        {
            try
            {
                _returnDictionary = publicService.GetAllSectorsAsync().Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                if (_returnDictionary["Result"] is not List<Sector> sectors) return BadRequest("Something went wrong! Could not acquire sectors.");

                var list = new List<dynamic>();
                foreach (var sector in sectors)
                {
                    list.Add(new
                    {
                        sector.Title,
                        Type = "Sector"
                    });
                }

                _returnDictionary = publicService.GetAllSoftwareProductsAsync().Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                if (_returnDictionary["Result"] is not List<SoftwareProduct> softwares) return BadRequest("Something went wrong! Could not acquire softwares.");

                foreach (var software in softwares)
                {
                    list.Add(new
                    {
                        Title = software.Name,
                        Type = "Software"
                    });
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}