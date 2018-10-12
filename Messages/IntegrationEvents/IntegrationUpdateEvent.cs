using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    /// <summary>
    /// Raised when integration data is updated.
    /// </summary>
    public class IntegrationUpdateEvent : IntegrationEvent
    {
        public string FtpHostname { get; set; }
        public string FtpPassword { get; set; }
        public int FtpPort { get; set; }
        public string FtpUsername { get; set; }
        public string GitUrl { get; set; }
        public string GitBranch { get; set; }
        public string FtpRootDirectory { get; set; }

        protected override void ValidateMe()
        {
            ValidateString(nameof(FtpHostname), FtpHostname);
            ValidateString(nameof(FtpPassword), FtpPassword);
            ValidateString(nameof(FtpUsername), FtpUsername);
            ValidateString(nameof(FtpRootDirectory), FtpRootDirectory);
            ValidateString(nameof(GitUrl), GitUrl);
            ValidateString(nameof(GitBranch), GitBranch);
            ValidatePort(FtpPort);            
        }
    }
}