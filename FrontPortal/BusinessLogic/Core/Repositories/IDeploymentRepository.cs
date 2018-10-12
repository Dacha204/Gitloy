using System;
using System.Threading.Tasks;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Repositories
{
    public interface IDeploymentRepository : IRepository<Deployment>
    {
        Deployment Get(Guid guid);
        Task<Deployment> GetAsync(Guid guid);
    }
}