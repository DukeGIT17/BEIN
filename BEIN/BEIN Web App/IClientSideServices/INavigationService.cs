using BEIN_DL.Models;

namespace BEIN_Web_App.IClientSideServices
{
    public interface INavigationService
    {
        Task<List<Sector>> GetAllSectorsAsync();
    }
}
