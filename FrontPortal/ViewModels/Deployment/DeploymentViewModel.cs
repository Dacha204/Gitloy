using System;

namespace Gitloy.Services.FrontPortal.ViewModels.Deployment
{
    public class DeploymentViewModel
    {
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string GitUrl { get; set; }
        public string GitBranch { get; set; } = "master";
        
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpHostname { get; set; }
        public int FtpPort { get; set; } = 21;
        public string FtpRootDirectory { get; set; } = "/";

        public DeploymentViewModel()
        {
        }

        public DeploymentViewModel(BusinessLogic.Core.Model.Deployment deployment)
        {
            Guid = deployment.Guid;
            FtpUsername = deployment.FtpUsername;
            FtpHostname = deployment.FtpHostname;
            GitUrl = deployment.GitUrl;
            GitBranch = deployment.GitBranch;
            FtpRootDirectory = deployment.FtpRootDirectory;
            FtpPassword = deployment.FtpPassword;
            FtpPort = deployment.FtpPort;
            CreatedAt = deployment.CreatedAt;
        }
    }
}