using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using System;

namespace appconfig_examples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        var settings = config.Build();

                        var connection = settings.GetConnectionString("AzureAppConfiguration");

                        config.AddAzureAppConfiguration(options => {
                            options.Connect(connection);

                            options.ConfigureRefresh(refresh =>
                            {
                                refresh.Register("daysToShow", refreshAll: true);
                                refresh.SetCacheExpiration(TimeSpan.FromSeconds(10));
                            });

                            options.Select(KeyFilter.Any, LabelFilter.Null);
                            options.Select(KeyFilter.Any, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
