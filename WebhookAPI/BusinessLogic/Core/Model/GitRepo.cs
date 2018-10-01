namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model
{
    public class GitRepo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Branch { get; set; } = "master";
        
        public virtual FtpNode FtpNode { get; set; }
        public int FtpNodeId { get; set; }
    }
}