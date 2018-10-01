using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs;
using Microsoft.EntityFrameworkCore;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore
{
    public class WebhookApiContext : DbContext
    {
        public DbSet<FtpNode> FtpNodes { get; set; }
        public DbSet<GitRepo> GitRepos { get; set; }
        public DbSet<Request> Requests { get; set; }
        
        public WebhookApiContext(DbContextOptions<WebhookApiContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FtpNodesConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}