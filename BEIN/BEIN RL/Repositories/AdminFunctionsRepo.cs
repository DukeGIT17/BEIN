using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Guid;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using Shared_Library.GlobalUtilities;

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

                if (sectors.Count == 0)
                    throw new("Could not find any sectors that matched the sector names provided.");

                product.Sectors = [];
                foreach (var sector in sectors)
                {
                    product.Sectors.Add(new()
                    {
                        SectorId = sector.Id,
                        SectorTitle = sector.Title,
                        Sector = sector,
                        ProductName = product.Name,
                        ProductId = product.Id
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
                _returnDictionary = FileUtilities.SaveFile(file, "Assets//Misc");
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string filePath = Path.Combine(env.ContentRootPath, "Assets//Misc", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var featureWS = package.Workbook.Worksheets[1];

                    var rowCount = worksheet.Dimension.Rows;
                    var featureRC = featureWS.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        SoftwareProduct sp = new()
                        {
                            Name = worksheet.Cells[row, 1].Value.ToString()!.Trim(),
                            Description = worksheet.Cells[row, 2].Value.ToString()!.Trim(),
                            Vendor = worksheet.Cells[row, 3].Value.ToString()!.Trim(),
                            ProjectStages = worksheet.GetListFromCell(row, 4, ','),
                            Professions = worksheet.GetListFromCell(row, 5, ','),
                            Sectors = [],
                            Features = []
                        };

                        var sectorTitles = worksheet.GetListFromCell(row, 6, ',');
                        foreach (Sector sector in context.Sectors)
                        {
                            if (sectorTitles.Contains(sector.Title))
                            {
                                sp.Sectors.Add(new()
                                {
                                    SectorId = sector.Id,
                                    ProductName = sp.Name,
                                    SectorTitle = sector.Title,
                                    ProductId = sp.Id
                                });
                            }
                        }

                        for (int fRow = 2; fRow <= featureRC; fRow++)
                        {
                            var spNames = featureWS.GetListFromCell(fRow, 3, ',');
                            if (spNames.Contains(sp.Name))
                            {
                                sp.Features.Add(new()
                                {
                                    Title = featureWS.Cells[fRow, 1].Value.ToString()!.Trim(),
                                    Description = featureWS.Cells[fRow, 2].Value.ToString()!.Trim(),
                                    SoftwareProductId = sp.Id
                                });
                            }
                        }

                        _returnDictionary = await AddSoftwareProductAsync(sp);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                string filePath = Path.Combine(env.ContentRootPath, "Assets//Misc", file.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}