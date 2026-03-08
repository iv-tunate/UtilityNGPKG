using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UtilityNGPKG.ExternalApiIntegration;
using UtilityNGPKG.FilePost;
using UtilityNGPKG.KYC;
using UtilityNGPKG.Mailer;
using UtilityNGPKG.Pagination;
using UtilityNGPKG.Pagination.UtilityNGPKG.Pagination;
using UtilityNGPKG.PaymentGateway.Paystack;
using UtilityNGPKG.Sanitizer;
using UtilityNGPKG.Tokenomics;

namespace UtilityNGPKG
{
    public static class Services
    {
        public static IServiceCollection AddUtilityNGPKG(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IApiIntegrationService, ExternalApiIntegration.IntegrationService>();
            services.AddScoped<IPaystackService, PaystackService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IKycService, KycService>();
            services.AddSingleton<IPaginationHelperFactory, PaginationHelperFactory>();
            services.AddSingleton<ITokenBuilder, TokenBuilder>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<ISanitizationService, SanitizationService>();
            return services;
        }
    }
}
