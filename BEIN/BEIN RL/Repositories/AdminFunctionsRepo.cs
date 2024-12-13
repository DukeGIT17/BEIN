using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Guid;

namespace BEIN_RL.Repositories
{
    public class AdminFunctionsRepo(BeinDbContext context) : IAdminFunctions
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> AddSectorAsync(Sector sector)
        {
            try
            {
                var existingSector = await context.Sectors.FirstOrDefaultAsync(s => s.Title == sector.Title);
                if (existingSector is not null) throw new($"A sector with the name '{sector.Title}' already exists.");

                sector.Id = NewGuid().ToString();
                sector.SectorInformation!.Id = NewGuid().ToString();
                sector.SectorInformation.CardInformation!.ForEach(ci => ci.Id = NewGuid().ToString());
                sector.SectorInformation.SectorPrinciples!.ForEach(sp => sp.Id = NewGuid().ToString());

                await context.AddAsync(sector);
                await context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> AddSoftwareProductAsync(SoftwareProduct product)
        {
            try
            {
                var existingProduct = await context.Softwares.FirstOrDefaultAsync(p => p.Name == product.Name);
                if (existingProduct is not null) throw new($"A software product with the name '{product.Name}' already exists.");

                List<Sector> sectors = [];
                foreach (var p in product.Sectors)
                    sectors.AddRange(context.Sectors.Where(s => s.Title == p.SectorTitle));

                if (sectors.IsNullOrEmpty())
                    throw new("Could not find any sectors that matched the sector names provided.");

                product.Sectors = [];
                foreach (var sector in sectors)
                {
                    product.Sectors.Add(new()
                    {
                        SectorId = sector.Id,
                        SectorTitle = sector.Title,
                        Sector = sector,
                    });
                }

                product.Id = NewGuid().ToString();
                await context.AddAsync(product);
                await context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
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
