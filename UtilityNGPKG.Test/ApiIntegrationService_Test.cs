using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UtilityNGPKG.Test.config;

namespace UtilityNGPKG.Test
{
    public class ApiIntegrationService_Test : BaseTestFeature
    {
        [Fact]
        public void SerializeReqBody_ProducesValidJsonContent()
        {
            var body = new { Name = "Test", Value = 42 };
            var content = apiIntegrationService.SerializeReqBody(body);

            Assert.NotNull(content);
            Assert.Equal("application/json", content.Headers.ContentType?.MediaType);
        }

        [SkippableFact]
        public async Task GetRequest_ReturnsSuccessResponse()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var response = await apiIntegrationService.GetRequest("https://httpbin.org/get");

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [SkippableFact]
        public async Task PostRequest_ReturnsSuccessResponse()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var body = apiIntegrationService.SerializeReqBody(new { test = true });
            var response = await apiIntegrationService.PostRequest(body, "https://httpbin.org/post");

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [SkippableFact]
        public async Task GetRequest_WithHeaders_PassesHeadersThrough()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var headers = new Dictionary<string, string>
            {
                { "X-Test-Header", "utility-ngpkg" }
            };

            var response = await apiIntegrationService.GetRequest("https://httpbin.org/headers", headers);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("utility-ngpkg", content);
        }
    }
}
