namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class FtpServer
    {
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public FtpAccount UserAccount { get; private set; }
        public string RootDirectory { get; private set; }
        
        public FtpServer(string hostname, int port, FtpAccount userAccount, string rootDirectory)
        {
            Hostname = hostname;
            Port = port;
            UserAccount = userAccount;
            RootDirectory = rootDirectory;
        }
    }
}