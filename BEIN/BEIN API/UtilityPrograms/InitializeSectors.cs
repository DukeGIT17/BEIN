using BEIN_DL.Data;
using BEIN_DL.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BEIN_API.UtilityPrograms
{
    internal static class InitializeSectors
    {
        internal static async Task InitializeSectorAsync(this WebApplication app, IWebHostEnvironment env)
        {
            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BeinDbContext>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = Path.Combine(Directory.GetParent(env.ContentRootPath)!.FullName, "Assets", "Sectors.xlsx");
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Sector sector = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = worksheet.Cells[row, 1].Value.ToString()!.Trim(),
                        ShortDescription = worksheet.Cells[row, 2].Value.ToString()!.Trim(),
                        Description = worksheet.Cells[row, 3].Value.ToString()!.Trim()
                    };

                    var s = await context.Sectors.FirstOrDefaultAsync(sp => sp.Title.ToLower() == sector.Title.ToLower());
                    if (s is null) await context.AddAsync(sector);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
