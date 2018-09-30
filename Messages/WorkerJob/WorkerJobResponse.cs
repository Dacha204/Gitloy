using Gitloy.BuildingBlocks.Messages.Data.Validation;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;

namespace Gitloy.BuildingBlocks.Messages.WorkerJob
{
    public class WorkerJobResponse : IValidate
    {
        public WorkerJobRequest Request { get; set; }
        public WorkerJobResultStatus ResultStatus { get; set; }
        public string ResultMessage { get; set; }
        public string JobOutput { get; set; }
        
        public WorkerJobResponse(WorkerJobRequest request, WorkerJobResultStatus resultStatus)
        {
            Request = request;
            ResultStatus = resultStatus;
        }

        public WorkerJobResponse()
        {
            ResultMessage = "Unknown";
            JobOutput = "Unknown";
        }

        public ValidationResult Validate()
        {
            if (Request == null)
                return ValidationResult.Invalid($"{nameof(Request)} is null");

            var requestValidation = Request.Validate();
            if (!requestValidation.IsValid)
                return requestValidation;
            
            if (ResultMessage == null)
                return ValidationResult.Invalid($"{ResultMessage} is null");
            
            if (JobOutput == null)
                return ValidationResult.Invalid($"{JobOutput} is null");
            
            return ValidationResult.Valid;
        }
    }
}