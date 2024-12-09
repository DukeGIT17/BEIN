using BEIN_DL.Models;

namespace BEIN_Web_App.IClientSideServices
{
    public interface IRequestService
    {
        Task<Dictionary<string, object>> SendRequestAsync<T>(T model, HttpMethod method, string endpoint);
        Task<Dictionary<string, object>> GetRequestAsync<T>(string endpoint);
        Task<Dictionary<string, object>> GetRequestAsync(string endpoint);
    }
}
