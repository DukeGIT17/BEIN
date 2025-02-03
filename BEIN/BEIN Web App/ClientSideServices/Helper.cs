using System.Net.Http.Headers;
using BEIN_DL.Models;

namespace BEIN_Web_App.ClientSideServices
{
    public static class Helper
    {
        /// <summary>
        /// Adds an IFormFile object to an existing MultipartFormDataContent object as a FileStreamContent object.
        /// </summary>
        /// <param name="file">File to be attached to the existing MultipartFormDataContent object.</param>
        /// <param name="formData">The existing MultipartFormDataContent object to attach the file to.</param>
        public static void AttachFileToMPFD(IFormFile file, MultipartFormDataContent formData)
        {
            var fileStreamContent = new StreamContent(file.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileStreamContent, "file", file.FileName);
        }
    }
}
