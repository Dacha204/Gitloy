using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class GitRepoConfiguration : IEntityTypeConfiguration<GitRepo>
    {
        public void Configure(EntityTypeBuilder<GitRepo> builder)
        {
            builder.Property(p => p.Url)
                .IsRequired();

            builder.HasOne<FtpNode>(p => p.FtpNode)
                .WithOne(g => g.GitRepo)
                .IsRequired();
        }
    }
}