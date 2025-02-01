using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace Shared_Library.GlobalUtilities
{
    public static class StaticUtilites
    {
        public static string ApplicationAssetsPath { get; } = "C:\\Users\\Lukhanyo\\Desktop\\Git Repo\\BEIN\\BEIN\\BEIN API\\Assets";

        public static bool IdentityOutcome(this IdentityResult result, out string error)
        {
            error = "";
            string err = "";
            result.Errors.ToList().ForEach(e => err += $"{e.Code}: {e.Description}\n");
            error = err;
            return !string.IsNullOrEmpty(error);
        }

        public static Dictionary<string, object> SaveFile(IFormFile file, string? destinationFolder = null, string? name = null)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                string fileName = file.FileName;
                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    if (name is not null)
                        fileName = name + Path.GetExtension(file.FileName);
                    else
                        fileName = file.FileName;
                }
                using var fileStream = new FileStream($@"{ApplicationAssetsPath}\{destinationFolder}\{fileName}", FileMode.Create);
                file.CopyTo(fileStream);

                _returnDictionary["Success"] = true;
                _returnDictionary["FileName"] = fileName;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public static Dictionary<string, object> DeleteFile(string filePath)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (!File.Exists(filePath)) throw new($"Could not find file path {filePath}.");
                File.Delete(filePath);

                _returnDictionary["Success"] = true;
                return _returnDictionary;
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
