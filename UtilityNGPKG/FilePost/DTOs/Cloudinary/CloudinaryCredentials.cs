using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.FilePost.DTOs.Cloudinary
{
    /// <summary>
    /// Represents the credentials required to authenticate with the Cloudinary API.
    /// </summary>
    /// <remarks>This class encapsulates the API key, API secret, and cloud name needed to access Cloudinary
    /// services. These credentials are typically provided by Cloudinary when you create an account and are required for
    /// most API operations.</remarks>
    public class CloudinaryCredentials
    {
        /// <summary>
        /// Gets or sets the API key used to authenticate requests to the Cloudinary service.
        /// </summary>
        public string CloudinaryAPIKey { get; set; }
        /// <summary>
        /// Gets or sets the API secret used to authenticate requests to the Cloudinary service.
        /// </summary>
        public string CloudinaryAPISecret { get;set; }
        /// <summary>
        /// Gets or sets the Cloudinary account name used for authentication and resource management.
        /// </summary>
        public string CloudinaryName { get; set; }
    }
}
