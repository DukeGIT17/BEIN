using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using BEIN_ServerSide_SL.IServerSideServices;
using Microsoft.AspNetCore.Http;

namespace BEIN_ServerSide_SL.ServerSideServices
{
    public class AdminFunctionsService(IAdminFunctions adminFunctions) : IAdminFunctionsService
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> AddSectorAsync(Sector sector)
        {
            try
            {
                return await adminFunctions.AddSectorAsync(sector);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> AddSoftwareProduct(SoftwareProduct product)
        {
            try
            {
                return await adminFunctions.AddSoftwareProductAsync(product);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> BulkSoftwareUpload(IFormFile file)
        {
            try
            {
                return await adminFunctions.BulkSoftwareUploadAsync(file);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
