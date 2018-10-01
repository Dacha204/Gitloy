using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers
{
    public abstract class GitEventHandler<TEvent> : IGitEventHandler<TEvent> 
        where TEvent : class
    {
        protected readonly IServiceScopeFactory ScopeFactory;
        // ReSharper disable once InconsistentNaming
        protected IUnitOfWork _uow;
        
        protected GitEventHandler(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
        }
        
        public void Handle(TEvent data)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                _uow = scope.ServiceProvider.GetService<IUnitOfWork>();
                HandleAction(data);
            }
        }

        public async Task HandleAsync(TEvent data)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                _uow = scope.ServiceProvider.GetService<IUnitOfWork>();
                await HandleActionAsync(data);
            }
        }

        protected abstract void HandleAction(TEvent data);
        protected abstract Task HandleActionAsync(TEvent data);
    }
}