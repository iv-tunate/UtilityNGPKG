using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.ExternalApiIntegration
{
    /// <summary>
    /// This interface defines a contract for a service that facilitates integration with external APIs by providing methods for sending HTTP requests and serializing request bodies. It abstracts the underlying HTTP client implementation, allowing for flexibility in how requests are made and enabling the addition of custom headers as needed.
    /// </summary>
    public interface IApiIntegrationService
    {
        /// <summary>
        /// Sends a POST request to the specified URL with optional headers.
        /// </summary>
        /// <param name="jsonBody">The JSON-formatted request body.</param>
        /// <param name="url">The endpoint to which the POST request is sent.</param>
        /// <param name="keyValuePairs">Optional headers to add to the request.</param>
        /// <param name="clientName">The named HTTP client to use (default is "default").</param>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the response.</returns>
        Task<HttpResponseMessage> PostRequest(StringContent jsonBody, string url, Dictionary<string, string>? keyValuePairs = null, string clientName = "default");

        /// <summary>
        /// Sends a GET request to the specified URL with optional headers.
        /// </summary>
        /// <param name="url">The endpoint to which the GET request is sent.</param>
        /// <param name="keyValuePairs">Optional headers to add to the request.</param>
        /// <param name="clientName">The named HTTP client to use (default is "default").</param>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the response.</returns>
        Task<HttpResponseMessage> GetRequest(string url, Dictionary<string, string>? keyValuePairs = null, string clientName = "default");

        /// <summary>
        /// Serializes an object into a JSON-formatted <see cref="StringContent"/> suitable for HTTP requests.
        /// </summary>
        /// <param name="reqBody">The request body object to serialize.</param>
        /// <returns>A <see cref="StringContent"/> instance containing the serialized JSON.</returns>
        StringContent SerializeReqBody(object reqBody);
    }
}
