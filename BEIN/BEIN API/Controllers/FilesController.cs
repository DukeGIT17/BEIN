using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared_Library.GlobalUtilities;

namespace BEIN_API.Controllers
{
    [Route("api.bein.com/[controller]")]
    [ApiController]
    public class FilesController(IWebHostEnvironment env) : ControllerBase
    {
        private Dictionary<string, object> _returnDictionary = [];

        /// <summary>
        /// BEIN API endpoint meant to service file save requests from clients. It excepts multipart/form-data content.
        /// </summary>
        /// <param name="file">
        /// The file that is to be saved.
        /// </param>
        /// <param name="name">
        /// The name with which to save the file (Optional).
        /// </param>
        /// <returns>Bad Request response on failure with an error message, or an empty Ok response on success.</returns>
        [HttpPost(nameof(SaveFile))]
        [Consumes("multipart/form-data")]
        public IActionResult SaveFile(IFormFile file, string? name)
        {
            try
            {
                _returnDictionary = FileUtilities.SaveFile(file, Path.Combine(env.ContentRootPath, "Assets", "Images"), name);
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"] as string);
                return Ok(_returnDictionary["FileName"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// BEIN API endpoint meant to service file deletion requests.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to be deleted.
        /// </param>
        /// <param name="fileType">
        /// The file type of the file to be deleted. Note: Supported files type are excel and image files.
        /// </param>
        /// <returns>Bad Request response on failure with an error message, or an empty Ok response on success.</returns>
        [HttpDelete(nameof(DeleteFile))]
        public IActionResult DeleteFile(string fileName, string fileType)
        {
            try
            {
                _returnDictionary = FileUtilities.DeleteFile(fileName, fileType);
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"] as string);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// BEIN API endpoint meant to service file retrieval requests
        /// </summary>
        /// <param name="filePath">The file path pointed to the location of the desired file.</param>
        /// <returns>Bad Request response with an error message on failure, and an Ok response with the desired file on success.</returns>
        [HttpGet(nameof(RetrieveFile))]
        public IActionResult RetrieveFile(string filePath)
        {
            try
            {
                _returnDictionary = FileUtilities.RetrieveFile(filePath);
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                if (_returnDictionary["File"] is not FileStreamResult result) return BadRequest("Something went wrong! Could not extract file stream result object from dictionary.");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
