using BEIN_DL.Models;

namespace BEIN_ServerSide_SL.IServerSideServices
{
    public interface IAccountService
    {
        Task<Dictionary<string, object>> RegisterUserAsync(RegistrationModel model);
        Task<Dictionary<string, object>> SignInAsync(SignInModel model);
        Task<Dictionary<string, object>> SignOutAsync();
    }
}
