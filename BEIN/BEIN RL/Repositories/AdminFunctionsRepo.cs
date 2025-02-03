using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Guid;
using static Shared_Library.GlobalUtilities.StaticUtilites;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;

namespace BEIN_RL.Repositories
{
    public class AdminFunctionsRepo(BeinDbContext context, IWebHostEnvironment env) : IAdminFunctions
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> AddSectorAsync(Sector sector)
        {
            try
            {
                var existingSector = await context.Sectors.FirstOrDefaultAsync(s => s.Title == sector.Title);
                if (existingSector is not null) throw new($"A sector with the name '{sector.Title}' already exists.");

                sector.Id = NewGuid().ToString();
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

        public async Task<Dictionary<string, object>> BulkSoftwareUploadAsync(IFormFile file)
        {
            try
            {
                _returnDictionary = SaveFile(file, "Misc");
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                using (var stream = new FileStream(Path.Combine(env.ContentRootPath, "Misc", file.FileName), FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var sectorWS = package.Workbook.Worksheets[1];
                    var featureWS = package.Workbook.Worksheets[2];

                    var rowCount = worksheet.Dimension.Rows;
                    var sectorRC = sectorWS.Dimension.Rows;
                    var featureRC = featureWS.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        SoftwareProduct sp = new()
                        {
                            Name = worksheet.Cells[row, 1].Value.ToString()!.Trim(),
                            Description = worksheet.Cells[row, 2].Value.ToString()!.Trim(),
                            Vendor = worksheet.Cells[row, 3].Value.ToString()!.Trim(),
                            ProjectStage = worksheet.Cells[row, 4].Value.ToString()!.Trim(),
                            Professions = worksheet.Cells[row, 5].Value.ToString()!.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(val => val.Trim()).ToList()
                            
                        };

                        sp.Sectors = [];
                        for (int sRow = 2; sRow <= sectorRC; sRow++)
                        {
                            sp.Sectors.Add(new()
                            {
                                ProductName = worksheet.Cells[row, 1].Value.ToString()!.Trim(),
                                SectorTitle = sectorWS.Cells[sRow, 1].Value.ToString()!.Trim()
                            });
                        }

                        sp.Features = [];
                        for (int fRow = 2; fRow <= featureRC; fRow++)
                        {
                            sp.Features.Add(new()
                            {
                                Title = worksheet.Cells[fRow, 1].Value.ToString()!.Trim(),
                                Description = worksheet.Cells[fRow, 2].Value.ToString()!.Trim(),
                            });
                        }

                        _returnDictionary = await AddSoftwareProductAsync(sp);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                }
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
