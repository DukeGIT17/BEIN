using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using BEIN_DL.Models;

namespace Shared_Library.GlobalUtilities
{
    public static class FileUtilities
    {
        /// <summary>
        /// Saves the provided file in the specified destination folder with the name provided. The value in the FileName property is used if the preferred name is not provided.
        /// </summary>
        /// <param name="file">The file to be saved.</param>
        /// <param name="dest">The destination folder in which the file is to be saved.</param>
        /// <param name="name">The name with which to save the provided file. (Optional)</param>
        /// <returns>
        /// Returns a <see cref="Dictionary{TKey, TValue}"/> containing a value indicating the success or failure of the operation,
        /// as well as the file name with which the file was saved with. <see cref="string"/>, TValue is <see cref="object"/>.
        /// </returns>
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
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a value indicating the success or failure of the operation,
        /// and an error message on failure. TKey is <see cref="string"/>, TValue is <see cref="object"/>.
        /// </returns>
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

        /// <summary>
        /// Extracts a <see cref="List{string}"/> of strings from the specified cell using the provided seperators.
        /// </summary>
        /// <param name="worksheet">The <see cref="ExcelWorksheet"/> from which the cell is located.</param>
        /// <param name="row">The row in which the cell with the information to be extracted is located.</param>
        /// <param name="column">The row in which the cell with the information to be extracted is located</param>
        /// <param name="separators">The separators with which to divide the string.</param>
        /// <returns>A <see cref="List{T}"/> extracted from the string in the target cell. T is <see cref="string"/></returns>
        public static List<string> GetListFromCell(this ExcelWorksheet worksheet, int row, int column, params char[] separators)
        {
            return worksheet.Cells[row, column].Value.ToString()!.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
        }

        /// <summary>
        /// Retrieves a file using the provided file path.
        /// </summary>
        /// <param name="filePath">The path pointing to the location of the desired file.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a value indicating the success or failure of the operation,
        /// and the <see cref="FileStreamResult"/> of the desired file. TKey is <see cref="string"/>, TValue is <see cref="object"/>.
        /// </returns>
        public static Dictionary<string, object> RetrieveFile(string filePath)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (!File.Exists(filePath))
                    throw new($"File Path {filePath} does not exist.");

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out string contentType)) contentType = "application/octet-stream";

                _returnDictionary["Success"] = true;
                _returnDictionary["File"] = new FileStreamResult(new FileStream(filePath, FileMode.Open, FileAccess.Read), contentType)
                {
                    FileDownloadName = Path.GetFileName(filePath)
                };
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
        /// Attempts to populate the ImageBase64 <see cref="string"/> property in each of the <see cref="SoftwareProduct"/> objects in the provided collection.
        /// </summary>
        /// <param name="softwares">The <see cref="SoftwareProduct"/> objects to populate with their images.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing a value indicating the success or failure of the operation, as well as the collection of
        /// <see cref="SoftwareProduct"/> objects whose ImageBase64 properties have been populated. TKey is <see cref="string"/>, TValue is <see cref="object"/>.
        /// </returns>
        public static Dictionary<string, object> PopulateWithBase64(this IEnumerable<SoftwareProduct> softwares)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                foreach (var software in softwares)
                {
                    string filePath = $@"Assets/Images/{software.ImageName}";
                    if (File.Exists(filePath))
                    {
                        var extension = software.ImageName![(software.ImageName!.LastIndexOf('.') + 1)..];
                        string base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));

                        software.ImageBase64 = $@"data:image/{extension};base64,{base64}";
                        continue;
                    }

                    software.ImageBase64 = $@"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes($@"Assets/Images/Default Image.png"))}";
                }

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = softwares;
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
