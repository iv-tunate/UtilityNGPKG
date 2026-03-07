using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.KYC.YouVerify
{
    /// <summary>
    /// Represents the credentials required to authenticate with the YouVerify API. This class contains properties for the base URL of the API and the secret key used for authentication. The BaseUrl property has a default value set to "https://api.youverify.co", which is the endpoint for the YouVerify API, while the SecretKey property must be provided by the user to successfully authenticate and interact with the API. This class serves as a simple data container for storing and managing the necessary credentials for accessing YouVerify's services.
    /// </summary>
    public class YouVerifyCredentials
    {
        /// <summary>
        /// The base URL for the YouVerify API. This property is initialized with a default value of "https://api.youverify.co", which is the standard endpoint for accessing YouVerify's services. Users can override this value if they need to point to a different environment (such as a staging or testing environment) or if YouVerify updates their API endpoint in the future. 
        /// The BaseUrl property is essential for constructing the full URLs for API requests when interacting with YouVerify's services.
        /// The base url will vary based on the environment you are using. For production, use "https://api.youverify.co". For testing, use "https://sandbox.youverify.co".
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.youverify.co";
        public string SecretKey { get; set; }
    }
}
