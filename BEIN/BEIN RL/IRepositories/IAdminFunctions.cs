using BEIN_DL.Models;
using Microsoft.AspNetCore.Http;

namespace BEIN_RL.IRepositories
{
    public interface IAdminFunctions
    {
        Task<Dictionary<string, object>> AddSectorAsync(Sector sector);
        Task<Dictionary<string, object>> AddSoftwareProductAsync(SoftwareProduct product);
        Task<Dictionary<string, object>> BulkSoftwareUploadAsync(IFormFile file);
    }
}
