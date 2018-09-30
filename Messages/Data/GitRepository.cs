using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class GitRepository : IValidate
    {
        public string Url { get; set; }
        public string Branch { get; set; }

        public GitRepository()
        {
            Url = "url";
            Branch = "branch";
        }

        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Url))
                return ValidationResult.Invalid($"{nameof(Url)} is empty");

            if (string.IsNullOrEmpty(Branch))
                return ValidationResult.Invalid($"{nameof(Branch)} is empty");
            
            return ValidationResult.Valid;
        }
    }
}