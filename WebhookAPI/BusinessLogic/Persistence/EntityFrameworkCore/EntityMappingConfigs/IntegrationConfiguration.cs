using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class IntegrationConfiguration : IEntityTypeConfiguration<Integration>
    {
        public void Configure(EntityTypeBuilder<Integration> builder)
        {
            builder.Property(p => p.FtpHostname)
                .IsRequired();

            builder.Property(p => p.FtpPassword)
                .IsRequired();
            
            builder.Property(p => p.GitUrl)
                .IsRequired();
        }
    }
}