using BEIN_DL.Models;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_API.Controllers
{
    [Route("api.bein.com/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService, IConfiguration configuration) : ControllerBase
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
                HttpContext.Response.Cookies.Append("AuthToken", _returnDictionary["Token"].ToString()!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtSettings:ExpirationInMinutes"))
                });
                return Ok(_returnDictionary["UserClaims"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(SignOut))]
        public new IActionResult SignOut()
        {
            try
            {
                var val = HttpContext.Response.Headers.SetCookie;
                HttpContext.Response.Cookies.Append("AuthToken", string.Empty, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
