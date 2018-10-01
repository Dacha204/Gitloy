using System.Windows.Input;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers;
using Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.EntityFrameworkCore;

namespace Gitloy.Services.WebhookAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            SetupBusCommunication(services);
            SetupPersistence(services);
            SetupBusinessLoggic(services);
        }

        private void SetupBusinessLoggic(IServiceCollection services)
        {
            services.AddScoped<IGitEventHandler<GithubPushEvent>, PushEventHandler>();
            services.AddScoped<IGitEventHandler<GithubPingEvent>, PingEventHandler>();
        }

        private void SetupBusCommunication(IServiceCollection services)
        {
            services.AddSingleton<ICommunicator, Communicator>();
        }

        private void SetupPersistence(IServiceCollection services)
        {
            services.AddDbContext<WebhookApiContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
 
            app.UseMvc();
        }
    }
}
