using System;
using System.Threading.Tasks;
using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.AspNetCore.Http;
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
                var git         = await LoadGit(data);
                var request     = await GenerateRequest(git);
                var jobRequest  = GenerateWorkerJobRequest(request.Id, git);
                var workerResponse = await _communicator.Bus.RequestAsync<WorkerJobRequest, WorkerJobResponse>(jobRequest);
                await ProcessResponse(request, workerResponse);
                
                //ToDo Notify that job has completed.
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                //Todo Notify that job has failed.
            }    
        }

        private async Task<GitRepo> LoadGit(GithubPushEvent data)
        {
            return await _uow.GitRepositories.SingleOrDefaultAsync(x => x.Url == data.Repository.CloneUrl) 
                   ?? throw new Exception($"Entry for git repo: {data.Repository.Url} not found");
        }
        
        private async Task<Request> GenerateRequest(GitRepo git)
        {
            var request = new Request()
            {
                Git = git,
                Status = RequestStatus.Requested,
                ResultStatus = RequestResultStatus.Pending,
            };

            await _uow.Requests.AddAsync(request);
            await _uow.CompleteAsync();
            return request;
        }

        private WorkerJobRequest GenerateWorkerJobRequest(int id, GitRepo git)
        {
            return new WorkerJobRequest()
            {
                Id = id,
                Status = WorkerJobStatus.Requested,
                GitRepository = new GitRepository()
                {
                    Url = git.Url,
                    Branch = git.Branch
                },
                FtpServer = new FtpServer()
                {
                    UserAccount = new FtpAccount()
                    {
                        Username = git.FtpNode.Username,
                        Password = git.FtpNode.Password
                    },
                    RootDirectory = git.FtpNode.RootDirectory,
                    Hostname = git.FtpNode.Hostname,
                    Port = git.FtpNode.Port
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