using System.Collections.Generic;
using System.Linq;
using Gitloy.Services.FrontPortal.ViewModels.DeploymentLog;

namespace Gitloy.Services.FrontPortal.ViewModels.Deployment
{
    public class DeploymentDetailsViewModel
    {
        public DeploymentViewModel Deployment { get; set; }
        public IList<DeploymentLogViewModel> Logs { get; set; }

        public DeploymentDetailsViewModel()
        {
        }

        public DeploymentDetailsViewModel(BusinessLogic.Core.Model.Deployment deployment)
        {
            Deployment = new DeploymentViewModel(deployment);
            Logs = deployment.Logs
                .ToList()
                .ConvertAll(l => new DeploymentLogViewModel(l));
        }
    }
}