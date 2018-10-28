using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.ViewModels;
using Gitloy.Services.FrontPortal.ViewModels.Deployment;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Handlers
{
    public class DeploymentHandler : IDeploymentHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeploymentHandler> _logger;
        private readonly WebhookAPIOptions _webhookOptions;
        private readonly ICommunicator _communicator;

        public DeploymentHandler(IUnitOfWork unitOfWork, 
            UserManager<IdentityUser> userManager,
            ILogger<DeploymentHandler> logger,
            IOptions<WebhookAPIOptions> webhookOptions,
            ICommunicator communicator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _webhookOptions = webhookOptions.Value;
            _communicator = communicator;
        }

        public IList<DeploymentViewModel> ListAll(ClaimsPrincipal user)
        {
            var deployments = _unitOfWork.Deployments.Find(
                x => x.User.Id == _userManager.GetUserId(user)).ToList();

            return deployments.ConvertAll(deploy => new DeploymentViewModel(deploy));
        }

        public DeploymentViewModel ViewDeployment(Guid deploymentGuid)
        {
            var deployment = _unitOfWork.Deployments.Get(deploymentGuid) ?? 
                throw new Exception($"Deployment with guid: {deploymentGuid} not found");

            return new DeploymentViewModel(deployment);
        }

        public Deployment CreateDeployment(DeploymentCreateViewModel deployment, ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserAsync(user).Result;

            CheckCreateConflicts(deployment);
            
            var newDeployment = new Deployment()
            {
                User = userId,
                FtpHostname = deployment.FtpHostname,
                FtpPassword = deployment.FtpPassword,
                FtpPort = deployment.FtpPort,
                FtpUsername = deployment.FtpUsername,
                GitBranch = deployment.GitBranch,
                GitUrl = deployment.GitUrl,
                FtpRootDirectory = deployment.FtpRootDirectory,
                State = DeploymentState.WaitingForPingEvent
            };
            
            _unitOfWork.Deployments.Add(newDeployment);
            
            newDeployment.Logs.Add(new DeploymentLog()
            {
                Description = "Created",
                Status = ResultStatus.Successful,
                DateTime = DateTime.Now,
                EventType = "create",
                ResultMessage = string.Empty,
            });
            
            _unitOfWork.Complete();

            _communicator.Bus.Publish(new IntegrationCreateEvent()
            {
                FtpHostname = newDeployment.FtpHostname,
                FtpPassword = newDeployment.FtpPassword,
                FtpPort = newDeployment.FtpPort,
                FtpUsername = newDeployment.FtpUsername,
                GitBranch = newDeployment.GitBranch,
                GitUrl = newDeployment.GitUrl,
                FtpRootDirectory = newDeployment.FtpRootDirectory,
                IntegrationGuid = newDeployment.Guid
            });
            
            return newDeployment;
        }
        
        public void UpdateDeployment(DeploymentViewModel deployment)
        {
            var dep = _unitOfWork.Deployments.Get(deployment.Guid) ??
                      throw new Exception($"Deployment with guid: {deployment.Guid} not found");

            CheckEditConflicts(deployment);
            
            dep.FtpHostname = deployment.FtpHostname;
            dep.FtpPort = deployment.FtpPort;
            dep.FtpUsername = deployment.FtpUsername;
            dep.FtpPassword = deployment.FtpPassword;
            dep.FtpRootDirectory = deployment.FtpRootDirectory;
            dep.GitBranch = deployment.GitBranch;
            dep.GitUrl = deployment.GitUrl;

            dep.Logs.Add(new DeploymentLog()
            {
                Description = "Changed",
                Status = ResultStatus.Successful,
                DateTime = DateTime.Now,
                EventType = "edit",
                ResultMessage = string.Empty
            });
            
            _unitOfWork.Complete();
            
            
            _communicator.Bus.Publish(new IntegrationUpdateEvent()
            {
                FtpHostname = dep.FtpHostname,
                FtpPassword = dep.FtpPassword,
                FtpPort = dep.FtpPort,
                FtpUsername = dep.FtpUsername,
                GitBranch = dep.GitBranch,
                GitUrl = dep.GitUrl,
                IntegrationGuid = dep.Guid,
                FtpRootDirectory = dep.FtpRootDirectory
            });
            
        }

        public void DeleteDeployment(Guid deploymentGuid)
        {
            var deployment = _unitOfWork.Deployments.Get(deploymentGuid) ??
                             throw new Exception($"Deployment with guid: {deploymentGuid} not found");
            
            _unitOfWork.Deployments.Remove(deployment);
            _unitOfWork.Complete();
            
            _communicator.Bus.Publish(new IntegrationDeleteEvent()
            {
                IntegrationGuid = deployment.Guid,
            });
        }

        public DeploymentDetailsViewModel GetDetails(Guid deploymentGuid)
        {
            var deployment = _unitOfWork.Deployments.Get(deploymentGuid) ??
                             throw new Exception($"Deployment with guid: {deploymentGuid} not found");
            
            return new DeploymentDetailsViewModel(deployment);
        }

        public WebhookParamsViewModel GenerateWebhookParams(Deployment deployment)
        {
            var urlParts = deployment.GitUrl.Split("/");
            if (urlParts.Length < 5)
            {
                var errorMessage = $"{deployment.GitUrl} is not a valid git url.";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            
            string user = urlParts[3];
            string repo = urlParts[4].Replace(".git", "");
            string createWebhookUrl = $"https://github.com/{user}/{repo}/settings/hooks/new";
           
            return new WebhookParamsViewModel()
            {
                DeploymentGuid = deployment.Guid,
                CreateWebhookURL = createWebhookUrl,
                Secret = string.Empty,
                ContentType = "application/json",
                PayloadURL = $"https://{_webhookOptions.Domain}{_webhookOptions.HookRoute}"
            };
        }
        
        private void CheckCreateConflicts(DeploymentCreateViewModel deployment)
        {
            bool conflictAlreadyLinkedGitAndFtp = _unitOfWork.Deployments.Any(x =>
                x.GitUrl == deployment.GitUrl &&
                x.GitBranch == deployment.GitBranch &&
                x.FtpHostname == deployment.FtpHostname &&
                x.FtpRootDirectory == deployment.FtpRootDirectory);
            
            if (conflictAlreadyLinkedGitAndFtp)
                throw new Exception("Git and FTP host already linked.");
            
        }

        private void CheckEditConflicts(DeploymentViewModel deployment)
        {
            bool conflictAlreadyLinkedGitAndFtp = _unitOfWork.Deployments.Count(x =>
                x.GitUrl == deployment.GitUrl &&
                x.GitBranch == deployment.GitBranch &&
                x.FtpHostname == deployment.FtpHostname &&
                x.FtpRootDirectory == deployment.FtpRootDirectory) > 1;
            
            if (conflictAlreadyLinkedGitAndFtp)
                throw new Exception("Git and FTP host already linked.");
        }
    }
}