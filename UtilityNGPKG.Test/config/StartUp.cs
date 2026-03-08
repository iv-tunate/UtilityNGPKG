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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config/testconfig.json", false, true)
                .Build();
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Debug);
            });
            services.AddUtilityNGPKG();
            return services.BuildServiceProvider();
        }

        public static T Resolve<T>() where T : notnull => provider.GetRequiredService<T>();
    }
}