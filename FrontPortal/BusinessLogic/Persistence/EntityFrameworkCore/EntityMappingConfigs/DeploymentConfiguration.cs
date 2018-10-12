using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
    {
        public void Configure(EntityTypeBuilder<Deployment> builder)
        {
            builder.Property(p => p.FtpHostname)
                .IsRequired();

            builder.Property(p => p.FtpPassword)
                .IsRequired();
            
            builder.Property(p => p.GitUrl)
                .IsRequired();

            builder.HasMany(p => p.Logs)
                .WithOne(p => p.Deployment);

            builder.HasOne(p => p.User);
        }
    }
}