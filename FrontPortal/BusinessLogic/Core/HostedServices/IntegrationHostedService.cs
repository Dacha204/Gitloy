using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.HostedServices
{
    public class IntegrationHostedService : IHostedService
    {
        private const string SUBSCRIPTION_ID = "FrontPortal";

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
                    .SubscribeAsync<IntegrationPingEvent>(SUBSCRIPTION_ID, OnIntegrationPingEvent));

            _subscriptions.Add(
                _communicator.Bus
                    .SubscribeAsync<IntegrationPushEvent>(SUBSCRIPTION_ID, OnIntegrationPushEvent));
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
        
        private async Task OnIntegrationPingEvent(IntegrationPingEvent @event)
        {
            try
            {
                _logger.LogInformation($"IntegrationPingEvent received. Guid:{@event.EventGuid}");
                
                ValidateReceivedIntegrationEvent(@event);
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var uof = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var deployment =
                        await uof.Deployments.GetAsync(@event.IntegrationGuid) ??
                        throw new Exception($"Deployment {@event.IntegrationGuid} not found in db");


                    deployment.State = DeploymentState.Verified;
                    deployment.Logs.Add(new DeploymentLog()
                    {
                        Description = "PingEvent received",
                        Status = ResultStatus.Successful,
                        DateTime = @event.DateTime,
                        EventType = "ping",
                    });
                    
                    await uof.CompleteAsync();
                    
                    _logger.LogInformation($"Deployment {deployment.Guid} activated");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private async Task OnIntegrationPushEvent(IntegrationPushEvent @event)
        {
            try
            {
                _logger.LogInformation($"IntegrationPushEvent received. Guid:{@event.EventGuid}");
                
                ValidateReceivedIntegrationEvent(@event);
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var uof = scope.ServiceProvider.GetService<IUnitOfWork>();


                    var deployment =
                        await uof.Deployments.GetAsync(@event.IntegrationGuid) ??
                        throw new Exception($"Deployment {@event.IntegrationGuid} not found in db");

                    deployment.Logs.Add(new DeploymentLog()
                    {
                        Description = @event.Details,
                        Status = @event.ResultStatus,
                        DateTime = @event.DateTime,
                        EventType = "push",
                        ResultMessage = @event.ResultMessage,
                        
                    });
                    
                    await uof.CompleteAsync();
                    
                    _logger.LogInformation($"Deployment {deployment.Guid} received push.");
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