using BEIN_DL.Data;
using BEIN_RL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace BEIN_RL.Repositories
{
    public class SectorRepo(BeinDbContext context) : ISector
    {
        private readonly Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> GetAllAsync()
        {
            try
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = await context.Sectors.ToListAsync();
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSectorAsync(string sectorName)
        {
            try
            {
                var sector = await context.Sectors.FirstOrDefaultAsync(s => s.Title == sectorName);
                if (sector is null) throw new($"Could not find a sector with the name {sectorName}.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = sector;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
