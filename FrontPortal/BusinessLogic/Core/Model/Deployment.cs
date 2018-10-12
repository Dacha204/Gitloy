using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Model
{
    public enum DeploymentState
    {
        WaitingForPingEvent,
        Verified
    }
    
    public class Deployment : Model
    {
        public DeploymentState State { get; set; }
        
        public string GitUrl { get; set; }
        public string GitBranch { get; set; } = "master";
        
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpHostname { get; set; }
        public int FtpPort { get; set; } = 21;
        public string FtpRootDirectory { get; set; } = "/";
        
        
        public virtual IList<DeploymentLog> Logs { get; set; }
        public virtual IdentityUser User { get; set; }
        
        public Deployment()
        {
            Logs = new List<DeploymentLog>();
            State = DeploymentState.WaitingForPingEvent;
        }
    }
}