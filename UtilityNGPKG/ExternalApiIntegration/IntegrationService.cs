using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.ExternalApiIntegration
{
    internal class IntegrationService : IApiIntegrationService
    {
        private readonly ILogger<IntegrationService> logger;
        private readonly IHttpClientFactory httpClient;
        public IntegrationService(ILogger<IntegrationService> logger, IHttpClientFactory httpClient)
        {
                this.httpClient = httpClient;
                this.logger = logger;
        }

        /// <summary>
        /// Serializes an object into a JSON-formatted <see cref="StringContent"/> suitable for HTTP requests.
        /// </summary>
        /// <param name="reqBody">The request body object to serialize.</param>
        /// <returns>A <see cref="StringContent"/> instance containing the serialized JSON.</returns>
        public StringContent SerializeReqBody(object reqBody)
        {
            var json = JsonConvert.SerializeObject(reqBody);
            var result = new StringContent(json, Encoding.UTF8, "application/json");
            return result;
        }

        /// <summary>
        /// Sends a POST request to the specified URL with optional headers.
        /// </summary>
        /// <param name="jsonBody">The JSON-formatted request body.</param>
        /// <param name="url">The endpoint to which the POST request is sent.</param>
        /// <param name="keyValuePairs">Optional headers to add to the request.</param>
        /// <param name="clientName">The named HTTP client to use (default is "default").</param>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the response.</returns>
        public async Task<HttpResponseMessage> PostRequest(StringContent jsonBody, string url, Dictionary<string, string>? keyValuePairs = null, string clientName = "default")
        {
            try
            {
                var client = httpClient.CreateClient(clientName);

                if (keyValuePairs != null)
                {
                    foreach (var kvp in keyValuePairs)
                    {
                        if (!client.DefaultRequestHeaders.Contains(kvp.Key))
                            client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                }

                var response = await client.PostAsync(url, jsonBody);
                return response;
            }
            catch(Exception ex) 
            {
                logger.LogCritical("{ex}", ex);
                return new HttpResponseMessage();
            }
        }

        /// <summary>
        /// Sends a GET request to the specified URL with optional headers.
        /// </summary>
        /// <param name="url">The endpoint to which the GET request is sent.</param>
        /// <param name="keyValuePairs">Optional headers to add to the request.</param>
        /// <param name="clientName">The named HTTP client to use (default is "default").</param>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the response.</returns>
        public async Task<HttpResponseMessage> GetRequest(string url, Dictionary<string, string>? keyValuePairs = null, string clientName = "default")
        {
            try
            {
                var client = httpClient.CreateClient(clientName);
                if (keyValuePairs != null)
                {
                    foreach (var kvp in keyValuePairs)
                    {
                        if (!client.DefaultRequestHeaders.Contains(kvp.Key))
                            client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                }
                var res = await client.GetAsync(url);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical("{ex}", ex);
                return new HttpResponseMessage();
            }
        }
    }
}
