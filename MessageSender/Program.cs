using System;
using System.Threading.Tasks;
using EasyNetQ;
using Gitloy.Services.Common.Communicator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageSender
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using (var host = BuildHost())
            {
                await host.StartAsync();

                var sender = host.Services.GetRequiredService<MessageSender>();
                sender.Start();

                await host.WaitForShutdownAsync();
            }
        }

        private static IHost BuildHost()
        {
            var host = new HostBuilder();
            
            ConfigureApp(host);
            ConfigureServices(host);

            return host.Build();
        }

        private static void ConfigureApp(IHostBuilder app)
        {
            app.ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
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
                services.AddSingleton<ICommunicator, Communicator>();
                services.AddSingleton<MessageSender>();
            });
        }
    }
}