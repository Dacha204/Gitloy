using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers
{
    public class PushEventHandler : GitEventHandler<GithubPushEvent>
    {
        private readonly ILogger<PushEventHandler> _logger;
        private readonly ICommunicator _communicator;
        
        public PushEventHandler(
            ILogger<PushEventHandler> logger,
            ICommunicator communicator,
            IServiceScopeFactory scopeFactory) 
            : base(scopeFactory)
        {
            _logger = logger;
            _communicator = communicator;
        }

        protected override void HandleAction(GithubPushEvent data)
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleActionAsync(GithubPushEvent data)
        {
            try
            {
                var integrations = LoadIntegrations(data);
                foreach (var integration in integrations)
                {
                    _logger.LogInformation($"Processing integration: " +
                                           $"[{integration.Id}] {integration.GitUrl} => " +
                                           $"{integration.FtpUsername}@{integration.FtpHostname}");
                    
                    await ProcessIntegration(integration);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                //Todo Notify that job has failed.
            }    
        }

        private async Task ProcessIntegration(Integration integration)
        {
            try
            {
                var request     = await GenerateRequest(integration);
                var jobRequest  = GenerateWorkerJobRequest(request.Id, integration);
                var workerResponse = await _communicator.Bus.RequestAsync<WorkerJobRequest, WorkerJobResponse>(jobRequest);
                await ProcessResponse(request, workerResponse);
                //ToDo Notify that job has completed.
                _logger.LogInformation($"Request {request.Id} processed. Result: {request.ResultStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        
        private List<Integration> LoadIntegrations(GithubPushEvent data)
        {
            var result = _uow.Integrations.Find(x => x.GitUrl == data.Repository.CloneUrl).ToList();
            
            if (result.IsNullOrEmpty())
            {
                _logger.LogError($"No integration found for '{data.Repository.CloneUrl}");
            }

            return result;
        }
        
        private async Task<Request> GenerateRequest(Integration integration)
        {
            var request = new Request()
            {
                Integration = integration,
                Status = RequestStatus.Requested,
                ResultStatus = RequestResultStatus.Pending,
            };

            await _uow.Requests.AddAsync(request);
            await _uow.CompleteAsync();
            return request;
        }

        private WorkerJobRequest GenerateWorkerJobRequest(int id, Integration integration)
        {
            return new WorkerJobRequest()
            {
                Id = id,
                Status = WorkerJobStatus.Requested,
                GitRepository = new GitRepository()
                {
                    Url = integration.GitUrl,
                    Branch = integration.GitBranch
                },
                FtpServer = new FtpServer()
                {
                    UserAccount = new FtpAccount()
                    {
                        Username = integration.FtpUsername,
                        Password = integration.FtpPassword
                    },
                    RootDirectory = integration.FtpRootDirectory,
                    Hostname = integration.FtpHostname,
                    Port = integration.FtpPort
                }
            };
        }
        
        private async Task ProcessResponse(Request request, WorkerJobResponse response)
        {
            request.Status = RequestStatus.Finished;
            
            request.ResultStatus = response.ResultStatus == WorkerJobResultStatus.Successful
                ? RequestResultStatus.Successful
                : RequestResultStatus.Failed;
            
            request.ResultMessage = response.ResultMessage;
            request.ResultDetails = response.JobOutput;
            await _uow.CompleteAsync();
        }
    }
}