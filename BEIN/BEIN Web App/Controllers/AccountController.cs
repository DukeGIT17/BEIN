using BEIN_Web_App.IClientSideServices;
using BEIN_Web_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace BEIN_Web_App.Controllers
{
    public class AccountController(IRequestService requestService, ILogger<AccountController> logger) : Controller
    {
        private Dictionary<string, object> _returnDictionary = [];

        [HttpGet]
        public IActionResult SignInOrRegister()
        {
            return View(new SignInAndRegistrationViewModel());
        }

        [HttpPost]
        public IActionResult Register(SignInAndRegistrationViewModel model)
        {
            try
            {
                if (model.RegistrationModel is null)
                {
                    ModelState.AddModelError("", "No registration data was provided.");
                    return View("SignInOrRegister", model);
                }

                if (ModelState.IsValid)
                {
                    _returnDictionary = requestService.SendRequestAsync(model.RegistrationModel, HttpMethod.Post, "/Account/Register").Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction("Index", "Home");
                }
                return View("SignInOrRegister", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SignIn(SignInAndRegistrationViewModel model)
        {
            try
            {
                if (model.SignInModel is null)
                {
                    ModelState.AddModelError("", "No sign in data was provided.");
                    return View("SignInOrRegister", model);
                }

                if (ModelState.IsValid)
                {
                    _returnDictionary = requestService.SendRequestAsync(model.SignInModel, HttpMethod.Post, "/Account/SignIn").Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction("landingPage", "General");
                }
                return View("SignInOrRegister", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public new IActionResult SignOut()
        {
            try
            {
                _returnDictionary = requestService.GetRequestAsync("/Account/SignOut").Result;
                if (!(bool)_returnDictionary["Success"]) throw new($"Critical Error, failed to log user '{HttpContext.User.Identity?.Name ?? "<No User>"}' out.");
                return RedirectToAction(nameof(SignInOrRegister));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
    