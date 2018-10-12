using System;
using Gitloy.BuildingBlocks.Messages.Data.Validation;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.BuildingBlocks.Messages.WorkerJob.Enums;
using Gitloy.Services.JobRunner.Shell;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.JobRunner
{
    public class JobRunner
    {
        public static WorkerJobResponse ExecuteJob(ILogger logger, WorkerJobRequest request)
        {
            try
            {
                using (var bash = new BashExecutor())
                {
                    ValidateRequest(request);
                
                    var command = $"/bin/bash ./Resources/runner.sh " +
                                  $"-h {request.FtpServer.Hostname} " +
                                  $"-P {request.FtpServer.Port} " +
                                  $"-d {request.FtpServer.RootDirectory} " +
                                  $"-u {request.FtpServer.UserAccount.Username} " +
                                  $"-p {request.FtpServer.UserAccount.Password} " +
                                  $"-g {request.GitRepository.Url} " +
                                  $"-b {request.GitRepository.Branch} " +
                                  $"-j {request.Id}";

                    bash.Execute("pwd");
                    var result = bash.Execute(command);

                    request.Status = WorkerJobStatus.Finished;
                
                    if (result.Successful)
                        return new WorkerJobResponse()
                        {
                            Request = request,
                            ResultMessage = $"Job successfully executed.",
                            JobOutput = result.Output,
                            ResultStatus = WorkerJobResultStatus.Successful
                        };
                
                    return new WorkerJobResponse()
                    {
                        Request = request,
                        ResultMessage = $"Job failed. Reason: {GetErrorMessage(result.ExitCode)}",
                        JobOutput = result.Output,
                        ResultStatus = WorkerJobResultStatus.Failed
                    };
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());

                request.Status = WorkerJobStatus.Finished;
                return new WorkerJobResponse()
                {
                    Request = request,
                    ResultMessage = e.Message,
                    ResultStatus = WorkerJobResultStatus.Failed,
                    JobOutput = e.StackTrace
                };
            }
            
        }

        private static string GetErrorMessage(int resultExitCode)
        {
            switch (resultExitCode)
            {
                case 2042:
                    return "Failed to create clone directory";
                case 2043:
                    return "Failed to clone repository";
                case 2044:
                    return "Failed to upload files";
                default:
                    return $"Unknown: ExitCode={resultExitCode}";
            }
        }

        private static void ValidateRequest(IValidate request)
        {
            var validationResult = request.Validate();
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Message);
        }
    }
}