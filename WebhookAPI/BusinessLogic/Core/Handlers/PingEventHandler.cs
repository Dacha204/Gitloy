using System;
using System.Threading.Tasks;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers
{
    public class PingEventHandler : GitEventHandler<GithubPingEvent>, IGitEventHandler<GithubPingEvent>
    {
        private readonly ILogger<PingEventHandler> _logger;
        private readonly ICommunicator _communicator;

        public PingEventHandler(
            ILogger<PingEventHandler> logger, 
            ICommunicator communicator, 
            IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _logger = logger;
            _communicator = communicator;
        }

        protected override void HandleAction(GithubPingEvent data)
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleActionAsync(GithubPingEvent data)
        {
            try
            {
                var git         = await LoadGit(data);
                //ToDo Notify that repo is setup correctly.
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private async Task<GitRepo> LoadGit(GithubPingEvent data)
        {
            return await _uow.GitRepositories.SingleOrDefaultAsync(x => x.Url == data.Repository.CloneUrl) 
                   ?? throw new Exception($"Entry for git repo: {data.Repository.Url} not found");
        }
    }
}