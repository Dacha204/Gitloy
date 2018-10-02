using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ValidationResult = Gitloy.BuildingBlocks.Messages.Data.Validation.ValidationResult;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    /// <summary>
    /// Raised when new integration is created.
    /// </summary>
    public class IntegrationCreateEvent : IntegrationEvent
    {
        public string FtpHostname { get; set; }
        public int FtpPort { get; set; }
        public string FtpPassword { get; set; }
        public string FtpUsername { get; set; }
        public string FtpRootDirectory { get; set; }
        public string GitBranch { get; set; }
        public string GitUrl { get; set; }
        
        protected override void ValidateMe()
        {
            ValidateString(nameof(FtpHostname), FtpHostname);
            ValidateString(nameof(FtpPassword), FtpPassword);
            ValidateString(nameof(FtpUsername), FtpUsername);
            ValidateString(nameof(FtpRootDirectory),  FtpUsername);
            ValidateString(nameof(GitBranch), GitBranch);
            ValidateString(nameof(GitUrl), GitUrl);
            ValidatePort(FtpPort);
        }
    }
}    