using System;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model
{
    public class Integration : Model
    {
        public string GitUrl { get; set; }
        public string GitBranch { get; set; } = "master";
        
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpHostname { get; set; }
        public int FtpPort { get; set; } = 21;
        public string FtpRootDirectory { get; set; } = "/";
    }
}