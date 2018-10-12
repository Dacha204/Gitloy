using System;
using System.Linq;
using System.Threading.Tasks;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Repositories
{
    public class DeploymentRepository : Repository<Deployment>, IDeploymentRepository
    {
        public DeploymentRepository(DbContext context) 
            : base(context)
        {
        }
        
        public Deployment Get(Guid guid)
        {
            return _dbSet.FirstOrDefault(x => x.Guid == guid && !x.DeleteFlag);
        }

        public async Task<Deployment> GetAsync(Guid guid)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Guid == guid && !x.DeleteFlag);
        }
    }
}