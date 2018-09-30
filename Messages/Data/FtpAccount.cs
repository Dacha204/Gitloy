using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class FtpAccount : IValidate
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public FtpAccount()
        {
            Username = "username";
            Password = "password";
        }

        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Username))
                return ValidationResult.Invalid("Username is empty");

            if (string.IsNullOrEmpty(Password))
                return ValidationResult.Invalid("Password is empty");

            return ValidationResult.Valid;
        }
    }
}