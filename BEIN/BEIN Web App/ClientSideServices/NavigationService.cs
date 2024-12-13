using BEIN_DL.Models;
using BEIN_Web_App.IClientSideServices;

namespace BEIN_Web_App.ClientSideServices
{
    public class NavigationService(IRequestService requestService) : INavigationService
    {
        public async Task<List<Sector>> GetAllSectorsAsync()
        {
            if (Helper.IsFirstLoad)
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
                Helper.IsFirstLoad = false;
            }

            var dic = await requestService.GetRequestAsync<List<Sector>>("/Sector/GetAll");
            if (!(bool)dic["Success"]) throw new(dic["ErrorMessage"] as string);
            if (dic["Result"] is not List<Sector> sectors) throw new("Failed to acquire the list of sectors.");
            return sectors;
        }
    }
}
