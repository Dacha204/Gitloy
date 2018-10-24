using System;
using Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            InitializeDatabase(webHost);
            
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables(prefix: "GITLOY_");
                })
                .UseStartup<Startup>();
        
        private static void InitializeDatabase(IWebHost webHost)
        {
            var logger = webHost.Services.GetRequiredService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Database Initialization Started");
                using (var scope = webHost.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    scope
                        .ServiceProvider
                        .GetRequiredService<WebhookApiContext>()
                        .Database
                        .Migrate();
                }

                logger.LogInformation("Database Initialization Finished");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Database Initialization Failed");
            }
        }
    }
}
