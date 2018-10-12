using System;
using System.Threading.Tasks;
using Gitloy.Services.FrontPortal.BusinessLogic.Core;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Repositories;
using Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Repositories;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FrontPortalDbContext _context;

        private readonly Lazy<IDeploymentRepository> _integrations;
        public IDeploymentRepository Deployments => _integrations.Value;
        
        private readonly Lazy<IRepository<DeploymentLog>> _deploymentLogs;
        public IRepository<DeploymentLog> DeploymentLogs => _deploymentLogs.Value;

        public UnitOfWork(FrontPortalDbContext context)
        {
            _context = context;
            _integrations = new Lazy<IDeploymentRepository>(() => new DeploymentRepository(_context));
            _deploymentLogs = new Lazy<IRepository<DeploymentLog>>(() => new Repository<DeploymentLog>(_context));
        }

        public int Complete() 
            => _context.SaveChanges();

        public async Task<int> CompleteAsync() 
            => await _context.SaveChangesAsync();
    }
}