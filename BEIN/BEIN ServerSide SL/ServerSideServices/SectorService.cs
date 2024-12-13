using BEIN_RL.IRepositories;
using BEIN_ServerSide_SL.IServerSideServices;

namespace BEIN_ServerSide_SL.ServerSideServices
{
    public class SectorService(ISector sector) : ISectorService
    {
        private readonly Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> GetAllAsync()
        {
            try
            {
                return await sector.GetAllAsync();
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
                return await sector.GetSectorAsync(sectorName);
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
