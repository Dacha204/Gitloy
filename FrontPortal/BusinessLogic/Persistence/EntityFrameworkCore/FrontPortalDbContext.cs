using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore
{
    public class FrontPortalDbContext : IdentityDbContext
    {
        public DbSet<Deployment> Deployments { get; set; }
        public DbSet<DeploymentLog> DeploymentLogs { get; set; }
        
        public FrontPortalDbContext(DbContextOptions<FrontPortalDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeploymentConfiguration());
            modelBuilder.ApplyConfiguration(new DeploymentLogsConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}