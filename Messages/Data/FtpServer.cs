using System.Net;
using System.Runtime.Serialization;

namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class FtpServer
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public FtpAccount UserAccount { get; set; }
        public string RootDirectory { get; set; }
        
        [IgnoreDataMember]
        public string Uri => $"ftp://{UserAccount.Username}:{UserAccount.Password}@{Hostname}/{RootDirectory}";
        
        public FtpServer()
        {
            Hostname = "hostname";
            UserAccount = new FtpAccount();
            RootDirectory = "/";
        }

    }
}