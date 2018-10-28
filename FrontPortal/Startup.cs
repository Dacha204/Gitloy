using Gitloy.Services.Common.Communicator;
using Gitloy.Services.FrontPortal.BusinessLogic.Core;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Handlers;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.HostedServices;
using Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore;
using Gitloy.Services.FrontPortal.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gitloy.Services.FrontPortal
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/Home/Docs");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            SetupBusCommunication(services);
            SetupPersistence(services);
            SetupBusinessLogic(services);
        }

        
        private void SetupPersistence(IServiceCollection services)
        {
            services.AddDbContext<FrontPortalDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services
                .AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<FrontPortalDbContext>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        
        private void SetupBusCommunication(IServiceCollection services)
        {
            services.AddCommunicator(Configuration);
            services.AddHostedService<IntegrationHostedService>();
        }

        
        private void SetupBusinessLogic(IServiceCollection services)
        {
            services.AddScoped<IDeploymentHandler, DeploymentHandler>();
            services.Configure<WebhookAPIOptions>(Configuration.GetSection("GitloyServices:WebhookAPI"));
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}