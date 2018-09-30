using System.Net;
using System.Runtime.Serialization;
using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class FtpServer : IValidate
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

        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Hostname))
                return ValidationResult.Invalid($"{nameof(Hostname)} is empty");
            
            if (string.IsNullOrEmpty(RootDirectory))
                return ValidationResult.Invalid($"{nameof(RootDirectory)} is empty");
            
            if (IPEndPoint.MinPort <= Port && Port >= IPEndPoint.MaxPort)
                return ValidationResult.Invalid($"Port is invalid");
            
            if (UserAccount == null)
                return ValidationResult.Invalid("FtpAccount is null");

            var accountValidation = UserAccount.Validate();
            if (!accountValidation.IsValid)
                return accountValidation;
            
            return ValidationResult.Valid;
        }
    }
}