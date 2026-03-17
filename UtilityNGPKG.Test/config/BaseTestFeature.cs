using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UtilityNGPKG.ExternalApiIntegration;
using UtilityNGPKG.FilePost;
using UtilityNGPKG.KYC;
using UtilityNGPKG.Mailer;
using UtilityNGPKG.Pagination;
using UtilityNGPKG.PaymentGateway.Paystack;
using UtilityNGPKG.Sanitizer;
using UtilityNGPKG.Tokenomics;

namespace UtilityNGPKG.Test.config
{
    public class BaseTestFeature
    {
        protected readonly IPaystackService paystackService;
        protected readonly IMailService mailService;
        protected readonly ISanitizationService sanitizationService;
        protected readonly ITokenBuilder tokenBuilder;
        protected readonly IKycService kycService;
        protected readonly IFileService fileService;
        protected readonly IApiIntegrationService apiIntegrationService;
        protected readonly IPaginationHelperFactory paginationHelperFactory;
        protected readonly IOptions<TestConfig> config;

        protected BaseTestFeature()
        {
            paystackService = StartUp.Resolve<IPaystackService>();
            mailService = StartUp.Resolve<IMailService>();
            sanitizationService = StartUp.Resolve<ISanitizationService>();
            tokenBuilder = StartUp.Resolve<ITokenBuilder>();
            kycService = StartUp.Resolve<IKycService>();
            fileService = StartUp.Resolve<IFileService>();
            apiIntegrationService = StartUp.Resolve<IApiIntegrationService>();
            paginationHelperFactory = StartUp.Resolve<IPaginationHelperFactory>();
            config = StartUp.Resolve<IOptions<TestConfig>>();
        }

        private static readonly Lazy<IConfiguration> _config = new(() =>
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config/testconfig.json", optional: false, reloadOnChange: true)
                .Build();
        });
        public static IConfiguration Config => _config.Value;
    }
}
