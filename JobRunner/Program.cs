using System.Threading.Tasks;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.JobRunner.HostedServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.JobRunner
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using (var host = BuildHost())
            {
                await host.RunAsync();
            }
        }

        private static IHost BuildHost()
        {
            var host = new HostBuilder();
            
            ConfigureApp(host);
            ConfigureServices(host);

            return host
                .Build();
        }

        private static void ConfigureApp(IHostBuilder app)
        {
            app.ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddEnvironmentVariables();
                config.AddEnvironmentVariables(prefix: "GITLOY_");
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
            });

            app.ConfigureLogging((hostContext, config) =>
            {
                config.AddConsole();
            });
            
            app.UseConsoleLifetime();
        }
        
        private static void ConfigureServices(HostBuilder app)
        {
            app.ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();
                services.AddCommunicator(hostContext.Configuration);
                services.AddHostedService<JobRunnerHostedService>();
            });
        }
    }
}