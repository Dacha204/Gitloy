using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
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
                var integrations = LoadIntegrations(data);
                //ToDo Notify that repo is setup correctly.
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private List<Integration> LoadIntegrations(GithubPingEvent data)
        {
            var result = _uow.Integrations.Find(x => x.GitUrl == data.Repository.CloneUrl).ToList();
            
            if (result.IsNullOrEmpty())
            {
                _logger.LogError($"No integration found for '{data.Repository.CloneUrl}");
            }

            return result;
        }
    }
}