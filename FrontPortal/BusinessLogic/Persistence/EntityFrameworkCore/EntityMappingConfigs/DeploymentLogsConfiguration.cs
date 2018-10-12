using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.EntityMappingConfigs
{
    public class DeploymentLogsConfiguration : IEntityTypeConfiguration<DeploymentLog>
    {
        public void Configure(EntityTypeBuilder<DeploymentLog> builder)
        {
            
        }
    }
}