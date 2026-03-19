using System;
using System.Threading.Tasks;
using UtilityNGPKG.KYC.YouVerify;
using UtilityNGPKG.Test.config;

namespace UtilityNGPKG.Test
{
    public class KycService_Test : BaseTestFeature
    {
        #region YouVerify

        private readonly string _baseUrl = "https://api.youverify.co";
        
        [SkippableFact]
        public async Task VerifyIdentificationNumber_BVN_ReturnsSuccess()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var details = new YouVerifyKycDto
            {
                Id = "11111111111",
                Type = DocumentType.BVN,
                IsSubjectConsent = true
            };

            var apiKey = config.Value.YouVerifyAPIKEY;
            var result = await kycService.VerifyIdentificationNumberAsync(details, _baseUrl, apiKey);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Status);
        }

        [SkippableFact]
        public async Task VerifyIdentificationNumber_NIN_ReturnsSuccess()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var details = new YouVerifyKycDto
            {
                Id = "11111111111",
                Type = DocumentType.NIN,
                IsSubjectConsent = true
            };

            var apiKey = config.Value.YouVerifyAPIKEY;
            var result = await kycService.VerifyIdentificationNumberAsync(details, _baseUrl, apiKey);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Status);
        }

        [SkippableFact]
        public async Task VerifyIdentificationNumber_Passport_ReturnsSuccess()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var details = new YouVerifyKycDto
            {
                Id = "A11111111",
                Type = DocumentType.InternationalPassport,
                LastName = "Doe",
                IsSubjectConsent = true
            };

            var apiKey = config.Value.YouVerifyAPIKEY;
            var result = await kycService.VerifyIdentificationNumberAsync(details, _baseUrl, apiKey);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Status);
        }

        #endregion
    }
}
