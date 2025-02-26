using BEIN_DL.Models;
using BEIN_Web_App.IClientSideServices;

namespace BEIN_Web_App.ClientSideServices
{
    public class NavigationService(IRequestService requestService, ILogger<NavigationService> logger) : INavigationService
    {
        public async Task<List<Sector>> GetAllSectorsAsync()
        {
            try
            {
                var dic = await requestService.GetRequestAsync<List<Sector>>("/Public/GetAllSectors");

                if (!(bool)dic["Success"])
                {
                    _ = dic.TryGetValue("ErrorMessage", out object? errorMessage);
                    if (errorMessage.ToString().StartsWith("No connection could be made"))
                    {
                        logger.LogWarning("Failed to connect to the server... Beginning second attempt...");

                        await Task.Delay(TimeSpan.FromSeconds(30));
                        dic = await requestService.GetRequestAsync<List<Sector>>("/Public/GetAllSectors");

                        if (!(bool)dic["Success"])
                            throw new(dic["ErrorMessage"] as string);
                    }
                    else
                        throw new(dic["ErrorMessage"] as string);
                }

                if (dic["Result"] is not List<Sector> sectors) 
                    throw new("Failed to acquire the list of sectors.");

                return sectors;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Second connection attempt failed... {ex.Message}");
                return [];
            }
        }
    }
}
