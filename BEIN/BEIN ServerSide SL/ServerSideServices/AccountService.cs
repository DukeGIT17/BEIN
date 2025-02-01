using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Identity;

namespace BEIN_ServerSide_SL.ServerSideServices
{
    public class AccountService(IAccount account, UserManager<IdentityUser> userManager) : IAccountService
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> RegisterUserAsync(RegistrationModel model)
        {
            try
            {
                foreach (var validator in userManager.PasswordValidators)
                {
                    var result = await validator.ValidateAsync(userManager, new IdentityUser(), model.Password);
                    if (!result.Succeeded)
                    {
                        string error = "";
                        result.Errors.ToList().ForEach(e => error = $"{e.Code}: {e.Description}");
                        throw new(error);
                    }
                }

                return await account.RegisterUserAsync(model);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> SignInAsync(SignInModel model)
        {
            try
            {
                return await account.SignInAsync(model);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
