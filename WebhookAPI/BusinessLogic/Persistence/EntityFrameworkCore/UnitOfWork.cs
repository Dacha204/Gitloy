using System;
using System.Threading.Tasks;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Repositories;
using Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.Repositories;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebhookApiContext _context;

        private readonly Lazy<IRepository<Integration>> _integrations;
        public IRepository<Integration> Integrations => _integrations.Value;

        private readonly Lazy<IRepository<Request>> _requests;
        public IRepository<Request> Requests => _requests.Value;

        public UnitOfWork(WebhookApiContext context)
        {
            _context = context;
            _integrations = new Lazy<IRepository<Integration>>(() => new Repository<Integration>(_context));
            _requests = new Lazy<IRepository<Request>>(() => new Repository<Request>(_context));
        }

        public int Complete() 
            => _context.SaveChanges();

        public async Task<int> CompleteAsync() 
            => await _context.SaveChangesAsync();
    }
}