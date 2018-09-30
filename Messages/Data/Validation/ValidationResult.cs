namespace Gitloy.BuildingBlocks.Messages.Data.Validation
{
    public class ValidationResult
    {
        public static ValidationResult Valid => new ValidationResult()
        {
            IsValid = true,
            Message = "OK"
        };
        
        public static ValidationResult Invalid(string reason) => new ValidationResult()
        {
            IsValid = false,
            Message = reason
        };
        
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}