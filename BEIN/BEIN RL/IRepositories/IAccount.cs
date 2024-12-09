using BEIN_DL.Models;

namespace BEIN_RL.IRepositories
{
    public interface IAccount
    {
        Task<Dictionary<string, object>> RegisterUserAsync(RegistrationModel model);
        Task<Dictionary<string, object>> SignInAsync(SignInModel model);
        Task<Dictionary<string, object>> SignOutAsync();
    }
}
