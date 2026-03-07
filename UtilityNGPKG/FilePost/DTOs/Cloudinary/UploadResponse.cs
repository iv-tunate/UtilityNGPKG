using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtilityNGPKG.FilePost.DTOs.Cloudinary
{
    /// <summary>
    /// Represents the response returned after attempting to upload a file to a storage provider. 
    /// This class contains properties that provide information about the uploaded file, including its public identifier, URL, success status, any error messages, and the file extension. 
    /// The properties are designed to capture the essential details of the upload operation and can be used by client code to determine the outcome of the upload and access the uploaded file if successful.
    /// </summary>
    public class UploadResponse
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }
        public string FileExtension { get; set; }
    }
}
