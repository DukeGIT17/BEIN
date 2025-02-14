using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using BEIN_ServerSide_SL.IServerSideServices;

namespace BEIN_ServerSide_SL.ServerSideServices
{
    public class PublicService(IPublic publicRepo) : IPublicService
    {
        private readonly Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> GetAllSectorsAsync()
        {
            try
            {
                return await publicRepo.GetAllSectorsAsync();
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAllSoftwareProductsAsync()
        {
            try
            {
                return await publicRepo.GetAllSoftwareProductsAsync();
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSectorAsync(string sectorName)
        {
            try
            {
                return await publicRepo.GetSectorAsync(sectorName);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSoftwareProductAsync(string softwareProductName)
        {
            try
            {
                return await publicRepo.GetSoftwareProductAsync(softwareProductName);
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