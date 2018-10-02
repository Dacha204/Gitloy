using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;
using Gitloy.Services.Common.Communicator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageSender
{
    public class MessageListener : IHostedService
    {
        private const string SUBSCRIPTION_ID = "DebugApp";
        
        private readonly ILogger<MessageListener> _logger;
        private readonly ICommunicator _communicator;
        private IList<ISubscriptionResult> _subscriptions;
        
        public MessageListener(ILogger<MessageListener> logger, ICommunicator communicator)
        {
            _logger = logger;
            _communicator = communicator;
            _subscriptions = new List<ISubscriptionResult>();
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            _subscriptions.Add(
                _communicator.Bus.Subscribe<IntegrationCreateEvent>(SUBSCRIPTION_ID, OnIntegrationEvent));

            _subscriptions.Add(
                _communicator.Bus.Subscribe<IntegrationDeleteEvent>(SUBSCRIPTION_ID, OnIntegrationEvent));

            _subscriptions.Add(
                _communicator.Bus.Subscribe<IntegrationPingEvent>(SUBSCRIPTION_ID, OnIntegrationEvent));

            _subscriptions.Add(
                _communicator.Bus.Subscribe<IntegrationPushEvent>(SUBSCRIPTION_ID, OnIntegrationEvent));
            
            _subscriptions.Add(
                _communicator.Bus.Subscribe<IntegrationUpdateEvent>(SUBSCRIPTION_ID, OnIntegrationEvent));
            
            _logger.LogInformation("MessageListener service started");
            return Task.CompletedTask;
        }

        private void OnIntegrationEvent(object data)
        {
            Console.WriteLine();
            Console.WriteLine("================");
            Console.WriteLine($"Received [{data.GetType().Name}]");
            Console.WriteLine("----------------");
            Console.WriteLine(JsonConvert.SerializeObject(data));
            Console.WriteLine("================");
            Console.WriteLine();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            
            _logger.LogInformation("MessageListener service stopped");
            return Task.CompletedTask;
        }
    }
}