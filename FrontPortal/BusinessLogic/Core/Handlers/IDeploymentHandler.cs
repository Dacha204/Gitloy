using System;
using System.Collections.Generic;
using System.Security.Claims;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.ViewModels;
using Gitloy.Services.FrontPortal.ViewModels.Deployment;

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
}