using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UtilityNGPKG.Test.config
{
    internal class StartUp
    {
        private static IServiceProvider provider;
        public StartUp()
        {
            provider = Build();
        }
        public static IServiceProvider Build()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config/testconfig.json", false, true)
                .Build();

            services.AddUtilityNGPKG();
            return services.BuildServiceProvider();
        }

        public static T Resolve<T>() where T : notnull => provider.GetRequiredService<T>();
    }
}
