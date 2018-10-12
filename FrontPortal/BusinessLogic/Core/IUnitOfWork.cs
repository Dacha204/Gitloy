using System.Threading.Tasks;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Repositories;
using Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Repositories;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core
{
    public interface IUnitOfWork
    {
        IDeploymentRepository Deployments { get; }
        IRepository<DeploymentLog> DeploymentLogs { get; }
        
        int Complete();
        Task<int> CompleteAsync();
    }
}