using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;

namespace Gitloy.BuildingBlocks.Messages.WorkerJob
{
    public class WorkerJobRequest
    {
        public int Id { get; private set; }
        public GitRepository GitRepository { get; private set; }
        public FtpServer FtpServer { get; private set; }
        public WorkerJobStatus Status { get; set; }

        public WorkerJobRequest(int id, GitRepository gitRepository, FtpServer ftpServer)
        {
            Id = id;
            GitRepository = gitRepository;
            FtpServer = ftpServer;
            Status = WorkerJobStatus.Requested;
        }
    }
}