using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace Shared_Library.GlobalUtilities
{
    public static class StaticUtilites
    {
        public static bool IdentityOutcome(this IdentityResult result, out string error)
        {
            error = "";
            string err = "";
            result.Errors.ToList().ForEach(e => err += $"{e.Code}: {e.Description}\n");
            error = err;
            return !string.IsNullOrEmpty(error);
        }

        public static Dictionary<string, object> SaveFile(IFormFile file, string dest, string? name = null)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                string fileName = file.FileName;
                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    if (name is not null)
                        fileName = $"{name} - {Guid.NewGuid() + Path.GetExtension(file.FileName)}";
                    else
                        fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)} - {Guid.NewGuid() + Path.GetExtension(file.FileName)}";
                }

                using var fileStream = new FileStream(Path.Combine(dest ?? "", fileName), FileMode.Create);
                file.CopyToAsync(fileStream).Wait();

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

        /// <summary>
        /// Permanent deletion of existing supported files (.xlsx, image).
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to be deleted.
        /// </param>
        /// <param name="fileType">
        /// The file type of the file to be deleted.
        /// </param>
        /// <returns></returns>
        public static Dictionary<string, object> DeleteFile(string fileName, string fileType)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (fileType == "Excel" || fileType == "Image")
                {
                    if (!File.Exists(fileName)) throw new($"Could not find file in the provided path.");
                    File.Delete(fileName);
                }
                else
                    throw new("Unsupported file type!");

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
