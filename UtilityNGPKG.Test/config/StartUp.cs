using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UtilityNGPKG.Test.config
{
    internal class StartUp
    {
        private static IServiceProvider provider;
        static StartUp()
        {
            provider = Build();
        }
        public static IServiceProvider Build()
        {
            var services = new ServiceCollection();
         
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Debug);
            });

            //var cfg = new ConfigurationBuilder()
            //    .SetBasePath(AppContext.BaseDirectory)
            //    .AddJsonFile("config/config.json", optional: true)
            //    .Build();

            //services.AddHttpClient("YouVerify", client =>
            //{
            //    var apiKey = cfg["YouVerifyAPIKEY"];
            //    if (!string.IsNullOrEmpty(apiKey))
            //        client.DefaultRequestHeaders.Add("token", apiKey);
            //});

            services.AddUtilityNGPKG();
            services.Configure<TestConfig>(BaseTestFeature.Config);
            return services.BuildServiceProvider();
        }

        public static T Resolve<T>() where T : notnull => provider.GetRequiredService<T>();
    }
}