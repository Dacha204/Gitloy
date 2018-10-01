using System;
using System.Threading.Tasks;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Repositories;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core
{
    public interface IUnitOfWork
    {
        IRepository<GitRepo> GitRepositories { get; }
        IRepository<Request> Requests { get; }
        
        int Complete();
        Task<int> CompleteAsync();
    }
}