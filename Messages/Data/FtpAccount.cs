namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class FtpAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public FtpAccount()
        {
            Username = "username";
            Password = "password";
        }
    }
}