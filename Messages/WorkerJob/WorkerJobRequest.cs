using System.Net;
using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.Data.Validation;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;

namespace Gitloy.BuildingBlocks.Messages.WorkerJob
{
    public class WorkerJobRequest : IValidate
    {
        public int Id { get; set; }
        public GitRepository GitRepository { get; set; }
        public FtpServer FtpServer { get; set; }
        public WorkerJobStatus Status { get; set; }

        public WorkerJobRequest(int id, GitRepository gitRepository, FtpServer ftpServer)
        {
            Id = id;
            GitRepository = gitRepository;
            FtpServer = ftpServer;
            Status = WorkerJobStatus.Requested;
        }

        public WorkerJobRequest()
        {
            GitRepository = new GitRepository();
            FtpServer = new FtpServer();
            Status = WorkerJobStatus.Requested;
        }

        public ValidationResult Validate()
        {
            if (GitRepository == null)
                return ValidationResult.Invalid($"{nameof(GitRepository)} is null");
            
            if (FtpServer == null)
                return ValidationResult.Invalid($"{nameof(GitRepository)} is null");
            
            var gitValidation = GitRepository.Validate();
            if (!gitValidation.IsValid)
                return gitValidation;

            var ftpValidation = FtpServer.Validate();
            if (!ftpValidation.IsValid)
                return ftpValidation;
            
            return ValidationResult.Valid;
        }
    }
}