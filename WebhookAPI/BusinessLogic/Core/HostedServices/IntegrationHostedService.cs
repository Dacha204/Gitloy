using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.HostedServices
{
    public class IntegrationHostedService : IHostedService
    {
        private const string SUBSCRIPTION_ID = "Webhook.API";

        private readonly ILogger<IntegrationHostedService> _logger;
        private readonly ICommunicator _communicator;
        private readonly IList<ISubscriptionResult> _subscriptions;
        private readonly IServiceScopeFactory _scopeFactory;

        public IntegrationHostedService(
            ILogger<IntegrationHostedService> logger,
            ICommunicator communicator,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _communicator = communicator;
            _subscriptions = new List<ISubscriptionResult>();
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RegisterListeners();

            _logger.LogInformation("IntegrationHostedService started");
            return Task.CompletedTask;
        }

        private void RegisterListeners()
        {
            _subscriptions.Add(
                _communicator.Bus
                    .SubscribeAsync<IntegrationCreateEvent>(SUBSCRIPTION_ID, OnIntegrationCreateEvent));

            _subscriptions.Add(
                _communicator.Bus
                    .SubscribeAsync<IntegrationDeleteEvent>(SUBSCRIPTION_ID, OnIntegrationDeleteEvent));

            _subscriptions.Add(
                _communicator.Bus
                    .SubscribeAsync<IntegrationUpdateEvent>(SUBSCRIPTION_ID, OnIntegrationUpdateEvent));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            _logger.LogInformation("IntegrationHostedService stopped");
            return Task.CompletedTask;
        }
        
        #region EventHandlers
        
        private async Task OnIntegrationUpdateEvent(IntegrationUpdateEvent @event)
        {
            try
            {
                _logger.LogInformation($"IntegrationUpdateEvent received. Guid:{@event.EventGuid}");
                
                ValidateReceivedIntegrationEvent(@event);
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var uof = scope.ServiceProvider.GetService<IUnitOfWork>();


                    var integration =
                        await uof.Integrations.SingleOrDefaultAsync(i => i.Guid == @event.IntegrationGuid) ??
                        throw new Exception($"Integration {@event.IntegrationGuid} not found in db");

                    integration.FtpHostname = @event.FtpHostname;
                    integration.FtpPassword = @event.FtpPassword;
                    integration.FtpPort = @event.FtpPort;
                    integration.FtpUsername = @event.FtpUsername;
                    integration.GitUrl = @event.GitUrl;
                    integration.GitBranch = @event.GitBranch;

                    await uof.CompleteAsync();

                    _logger.LogInformation($"Integration {integration.Guid} updated");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private async Task OnIntegrationDeleteEvent(IntegrationDeleteEvent @event)
        {
            try
            {
                _logger.LogInformation($"IntegrationDeleteEvent received. Guid:{@event.EventGuid}");
                
                ValidateReceivedIntegrationEvent(@event);
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var uof = scope.ServiceProvider.GetService<IUnitOfWork>();


                    var integration =
                        await uof.Integrations.SingleOrDefaultAsync(i => i.Guid == @event.IntegrationGuid) ??
                        throw new Exception($"Integration {@event.IntegrationGuid} not found in db");

                    uof.Integrations.Remove(integration);
                    await uof.CompleteAsync();

                    _logger.LogInformation($"Integration {integration.Guid} removed/deactivated.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private async Task OnIntegrationCreateEvent(IntegrationCreateEvent @event)
        {
            try
            {
                _logger.LogInformation($"IntegrationCreateEvent received. Guid: {@event.EventGuid}");
                
                ValidateReceivedIntegrationEvent(@event);
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var uof = scope.ServiceProvider.GetService<IUnitOfWork>();


                    if (uof.Integrations.Exists(i => i.Guid == @event.IntegrationGuid))
                        throw new Exception($"Integration already exists: {@event.IntegrationGuid}");

                    var integration = new Integration()
                    {
                        Guid = @event.IntegrationGuid,
                        FtpHostname = @event.FtpHostname,
                        FtpPort = @event.FtpPort,
                        FtpPassword = @event.FtpPassword,
                        FtpUsername = @event.FtpUsername,
                        FtpRootDirectory = @event.FtpRootDirectory,
                        GitBranch = @event.GitBranch,
                        GitUrl = @event.GitUrl
                    };

                    uof.Integrations.Add(integration);
                    await uof.CompleteAsync();

                    _logger.LogInformation($"Integration {integration.Guid} created");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private void ValidateReceivedIntegrationEvent(IntegrationEvent @event)
        {
            var result = @event.Validate();
            if (!result.IsValid)
                throw new ArgumentException(
                    $"Received IntegrationEvent '{@event.EventGuid}' is not valid. " +
                    $"Reason: {result.Message}");
        }
        
        #endregion
    }
}