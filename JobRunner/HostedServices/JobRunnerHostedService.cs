using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;
using Gitloy.Services.Common.Communicator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.JobRunner.HostedServices
{
    public class JobRunnerHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ICommunicator _comm;
        private readonly List<IDisposable> _subscriptions;
        
        public JobRunnerHostedService(ICommunicator communicator, ILogger<JobRunnerHostedService> logger)
        {
            _logger = logger;
            _comm = communicator;
            _subscriptions = new List<IDisposable>();
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("JobRunnerHostedService started");
            
            _comm.Connect();
            RegisterResponders(cancellationToken);
            
            return Task.CompletedTask;
        }

        private void RegisterResponders(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering responders.");
            
            _subscriptions.Add(
                _comm.Bus.RespondAsync<WorkerJobRequest, WorkerJobResponse>(request =>
                    Task.Factory.StartNew(() => JobRunner.ExecuteJob(_logger, request))));
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Runner Hosted Service is stopping.");

            foreach (var subscription in _subscriptions)
                subscription.Dispose();
            
            _subscriptions.Clear();

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            _comm?.Dispose();
        }
    }
}