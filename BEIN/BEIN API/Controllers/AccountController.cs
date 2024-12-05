using BEIN_DL.Models;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_API.Controllers
{
    [Route("bein.com/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(Register))]
        public IActionResult Register(RegistrationModel model)
        {
            try
            {
                _returnDictionary = accountService.RegisterUserAsync(model).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(SignIn))]
        public IActionResult SignIn(SignInModel model)
        {
            try
            {
                _returnDictionary = accountService.SignInAsync(model).Result;
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
