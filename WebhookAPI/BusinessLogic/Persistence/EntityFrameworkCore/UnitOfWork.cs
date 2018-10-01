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

        private readonly Lazy<IRepository<GitRepo>> _gits;
        public IRepository<GitRepo> GitRepositories => _gits.Value;

        private readonly Lazy<IRepository<Request>> _requests;
        public IRepository<Request> Requests => _requests.Value;

        public UnitOfWork(WebhookApiContext context)
        {
            _context = context;
            _gits = new Lazy<IRepository<GitRepo>>(() => new Repository<GitRepo>(_context));
            _requests = new Lazy<IRepository<Request>>(() => new Repository<Request>(_context));
        }

        public int Complete() 
            => _context.SaveChanges();

        public async Task<int> CompleteAsync() 
            => await _context.SaveChangesAsync();
    }
}