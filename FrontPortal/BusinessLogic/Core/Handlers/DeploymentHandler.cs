using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.Controllers;
using Gitloy.Services.FrontPortal.ViewModels;
using Gitloy.Services.FrontPortal.ViewModels.Deployment;
using Gitloy.Services.FrontPortal.ViewModels.DeploymentLog;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Handlers
{
    public interface IDeploymentHandler
    {
        IList<DeploymentViewModel> ListAll(ClaimsPrincipal user);
        DeploymentDetailsViewModel GetDetails(Guid deploymentGuid);
        Deployment CreateDeployment(DeploymentCreateViewModel deployment, ClaimsPrincipal user);
        DeploymentViewModel ViewDeployment(Guid deploymentGuid);
        void UpdateDeployment(DeploymentViewModel deployment);
        void DeleteDeployment(Guid deploymentGuid);
        WebhookParamsViewModel GenerateWebhookParams(Deployment deployment);
    }
    
    public class DeploymentHandler : IDeploymentHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeploymentHandler> _logger;
        private readonly IConfiguration _configuration;

        public DeploymentHandler(IUnitOfWork unitOfWork, 
            UserManager<IdentityUser> userManager,
            ILogger<DeploymentHandler> logger,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
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
            };
            
            _unitOfWork.Deployments.Add(newDeployment);
            _unitOfWork.Complete();

            return newDeployment;
        }
        
        public void UpdateDeployment(DeploymentViewModel deployment)
        {
            var dep = _unitOfWork.Deployments.Get(deployment.Guid) ??
                      throw new Exception($"Deployment with guid: {deployment.Guid} not found");;

            CheckEditConflicts(deployment);
            
            dep.FtpHostname = deployment.FtpHostname;
            dep.FtpPort = deployment.FtpPort;
            dep.FtpUsername = deployment.FtpUsername;
            dep.FtpPassword = deployment.FtpPassword;
            dep.FtpRootDirectory = deployment.FtpRootDirectory;
            dep.GitBranch = deployment.GitBranch;
            dep.GitUrl = deployment.GitUrl;

            _unitOfWork.Complete();
        }

        public void DeleteDeployment(Guid deploymentGuid)
        {
            var deployment = _unitOfWork.Deployments.Get(deploymentGuid) ??
                             throw new Exception($"Deployment with guid: {deploymentGuid} not found");
            
            _unitOfWork.Deployments.Remove(deployment);
            _unitOfWork.Complete();
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
            string user = urlParts[3];
            string repo = urlParts[4].Replace(".git", "");
            string createWebhookUrl = $"https://github.com/{user}/{repo}/settings/hooks/new";

            return new WebhookParamsViewModel()
            {
                DeploymentGuid = deployment.Guid,
                CreateWebhookURL = createWebhookUrl,
                Secret = string.Empty,
                ContentType = "application/json",
                PayloadURL = _configuration["GitloyServices:WebhookAPI:URL"]
            };
        }
        
        private void CheckCreateConflicts(DeploymentCreateViewModel deployment)
        {
            bool conflictAlreadyLinkedGitAndFtp = _unitOfWork.Deployments.Any(x =>
                x.GitUrl == deployment.GitUrl &&
                x.FtpHostname == deployment.FtpHostname &&
                x.FtpRootDirectory == deployment.FtpRootDirectory);
            
            if (conflictAlreadyLinkedGitAndFtp)
                throw new Exception("Git and FTP host already linked.");
            
        }

        private void CheckEditConflicts(DeploymentViewModel deployment)
        {
            bool conflictAlreadyLinkedGitAndFtp = _unitOfWork.Deployments.Any(x =>
                x.GitUrl == deployment.GitUrl &&
                x.FtpHostname == deployment.FtpHostname &&
                x.FtpRootDirectory == deployment.FtpRootDirectory);
            
            if (conflictAlreadyLinkedGitAndFtp)
                throw new Exception("Git and FTP host already linked.");
        }
    }
}