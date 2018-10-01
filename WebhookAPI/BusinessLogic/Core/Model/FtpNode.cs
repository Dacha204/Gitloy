namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model
{
    public class FtpNode
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; } = 21;
        public string RootDirectory { get; set; } = "/";
        
        public virtual GitRepo GitRepo { get; set; }
    }
}