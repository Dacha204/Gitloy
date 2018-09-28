using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;

namespace Gitloy.BuildingBlocks.Messages.WorkerJob
{
    public class WorkerJobResponse
    {
        public WorkerJobRequest Request { get; private set; }
        public WorkerJobResultStatus ResultStatus { get; set; }
        public string ResultMessage { get; set; }
        public string JobOutput { get; set; }
        
        public WorkerJobResponse(WorkerJobRequest request, WorkerJobResultStatus resultStatus)
        {
            Request = request;
            ResultStatus = resultStatus;
        }
    }
}