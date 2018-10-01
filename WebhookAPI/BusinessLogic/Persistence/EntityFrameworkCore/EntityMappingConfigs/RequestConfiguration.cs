using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
        }
    }
}