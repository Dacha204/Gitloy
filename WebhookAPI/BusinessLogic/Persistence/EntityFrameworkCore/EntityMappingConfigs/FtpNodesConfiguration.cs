using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class FtpNodesConfiguration : IEntityTypeConfiguration<FtpNode>
    {
        public void Configure(EntityTypeBuilder<FtpNode> builder)
        {
            builder.Property(p => p.Hostname)
                .IsRequired();

            builder.Property(p => p.Password)
                .IsRequired();
        }
    }
}