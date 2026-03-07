using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.FilePost.DTOs.Cloudinary
{
    /// <summary>
    /// This class represents the body of a file upload request. It contains properties that define the constraints and parameters for uploading files to Cloudinary.
    /// </summary>
    public class UploadRequestBody
    {
        public int FileLimit { get; set; }
        public string CloudinaryBaseFolder { get; set; }
        public string UniqueIdentifier { get; set; } = "";
        /// <summary>
        /// This represents the directory you would like the file or document to be uploaded to. 
        /// It determines the path/directory in which the file is saved. For example, scripts should have a document type of "scripts". Likewise images should have a document type of images.
        /// The result will be "yourcloudinarybasefolder/uniqueidentifier/directoryname"If nothing is specified, it defaults to documents
        /// </summary>
        public string DirectoryName { get; set; } = "documents";
        public List<string> AllowedMimeTypes { get; set; }
        public List<string> AllowedExtensionTypes { get; set; }
    }
}
