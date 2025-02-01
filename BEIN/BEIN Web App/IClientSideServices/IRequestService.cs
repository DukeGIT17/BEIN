using BEIN_DL.Models;

namespace BEIN_Web_App.IClientSideServices
{
    public interface IRequestService
    {
        Task<Dictionary<string, object>> SendRequestAsync<T>(T model, HttpMethod method, string endpoint);
        Task<Dictionary<string, object>> SignInRequestAsync(SignInModel model);
        Task<Dictionary<string, object>> SendFileAsync(IFormFile file, HttpMethod method, string endpoint);
        Task<Dictionary<string, object>> GetRequestAsync<T>(string endpoint);
        Task<Dictionary<string, object>> GetRequestAsync(string endpoint);
        Task<Dictionary<string, object>> SignOutRequestAsync();
    }
}
