using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gitloy.Services.Common.Communicator
{
    public static class CommunicatorConfiguration
    {
        public static void AddCommunicator(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EasyNetQOptions>(configuration.GetSection("EasyNetQ"));
            services.AddSingleton<ICommunicator, Communicator>();
        }
    }
}