using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared_Library.GlobalUtilities;

namespace BEIN_RL.Repositories
{
    public class PublicRepo(BeinDbContext context) : IPublic
    {
        private Dictionary<string, object> _returnDictionary = [];

        /// <summary>
        /// Accesses the BEIN Database and retrieves all available sectors.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a key value pairs indicating the success or failure of the operation, 
        /// as well as the result of the operation, a <see cref="List{T}"/> of sectors. T is <seealso cref="Sector"/>.
        /// </returns>
        public async Task<Dictionary<string, object>> GetAllSectorsAsync()
        {
            try
            {
                var sectors = await context.Sectors.AsNoTracking().Include(p => p.Products).ThenInclude(p => p.Product).ToListAsync();

                foreach (var sector in sectors)
                {
                    if (sector.Products is null)
                        continue;

                    foreach (var sp in sector.Products)
                    {
                        var softwares = new List<SoftwareProduct> { sp.Product! };
                        _returnDictionary = softwares.PopulateWithBase64();
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                        if (_returnDictionary["Result"] is not List<SoftwareProduct> softwareList) 
                            throw new($"Failed to acquire one of the softwares from {sp.SectorTitle} sector.");

                        sp.Product = softwareList[0];
                    }
                }

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = sectors;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        /// <summary>
        /// Accesses the BEIN Database and retrieves all available software products.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a key value pairs indicating the success or failure of the operation, 
        /// as well as the result of the operation, a <see cref="List{T}"/> of software products. T is <seealso cref="SoftwareProduct"/>.
        /// </returns>
        public async Task<Dictionary<string, object>> GetAllSoftwareProductsAsync()
        {
            try
            {
                var softwares = await context.Softwares
                    .AsNoTracking()
                    .Include(s => s.Sectors)
                    .Include(f => f.Features)
                    .Include(v => v.Visits)
                    .ToListAsync();

                if (softwares.Count == 0 || softwares is null) throw new("No software found!");

                _returnDictionary = softwares.PopulateWithBase64();
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        /// <summary>
        /// Accesses the BEIN Database and retrieves the specified sector, if it exists.
        /// </summary>
        /// <param name="sectorName">The title of the sector to be retrieved.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a key value pairs indicating the success or failure of the operation, 
        /// as well as the result of the operation, a <see cref="Sector"/> model with its associated products.
        /// </returns>
        public async Task<Dictionary<string, object>> GetSectorAsync(string sectorName)
        {
            try
            {
                var sector = await context.Sectors.AsNoTracking().Include(p => p.Products).ThenInclude(p => p.Product).FirstOrDefaultAsync(s => s.Title.ToLower() == sectorName.ToLower());
                if (sector is null) throw new($"Could not find a sector with the name {sectorName}.");

                if (sector.Products is not null)
                {
                    foreach (var sp in sector.Products!)
                    {
                        var softwares = new List<SoftwareProduct> { sp.Product! };
                        _returnDictionary = softwares.PopulateWithBase64();
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                        if (_returnDictionary["Result"] is not List<SoftwareProduct> softwareList)
                            throw new($"Failed to acquire one of the softwares from {sp.SectorTitle} sector.");

                        sp.Product = softwareList[0];
                    }
                }

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

        /// <summary>
        /// Accesses the BEIN Database and retrieves the specified software product, if it exists.
        /// </summary>
        /// <param name="softwareProductName">The name of the software to retrieve.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a key value pairs indicating the success or failure of the operation, 
        /// as well as the result of the operation, a <see cref="SoftwareProduct"/> model and all its associated navigation properties.
        /// </returns>
        public async Task<Dictionary<string, object>> GetSoftwareProductAsync(string softwareProductName)
        {
            try
            {
                var software = await context.Softwares
                    .AsNoTracking()
                    .Include(s => s.Sectors)
                    .Include(f => f.Features)
                    .Include(v => v.Visits)
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == softwareProductName.ToLower());
                if (software is null) throw new($"Could not find a software product with the name {softwareProductName}.");

                var softwares = new List<SoftwareProduct> { software };
                _returnDictionary = softwares.PopulateWithBase64();
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
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
