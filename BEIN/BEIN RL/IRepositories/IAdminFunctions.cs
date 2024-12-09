using BEIN_DL.Models;

namespace BEIN_RL.IRepositories
{
    public interface IAdminFunctions
    {
        Task<Dictionary<string, object>> AddSectorAsync(Sector sector);
        Task<Dictionary<string, object>> AddSoftwareProductAsync(SoftwareProduct product);
    }
}
