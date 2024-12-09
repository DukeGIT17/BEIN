using BEIN_DL.Models;

namespace BEIN_ServerSide_SL.IServerSideServices
{
    public interface IAdminFunctionsService
    {
        Task<Dictionary<string, object>> AddSectorAsync(Sector sector);
        Task<Dictionary<string, object>> AddSoftwareProduct(SoftwareProduct product);
    }
}
