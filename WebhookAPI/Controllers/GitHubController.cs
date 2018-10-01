using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI.Controllers
{
    [ApiController]
    [Route("api/webhooks/incoming/github")]
    public class GitHubController : ControllerBase
    {
        private readonly ILogger<GitHubController> _logger;
        private readonly ICommunicator _communicator;

        public GitHubController(ILogger<GitHubController> logger, ICommunicator communicator)
        {
            _logger = logger;
            _communicator = communicator;
        }

        [HttpPost]
        public IActionResult Webhook()
        {
            string responseMessage;

            try
            {
                var payload = GithubPayloadExtractor.Extract(Request);

                switch (payload.Event)
                {
                    case "push":
                        GithubHandle((PushEvent) payload.Result);
                        responseMessage = "Gitloy: PingEvent received.";
                        break;
                    case "ping":
                        GithubHandle((PingEvent) payload.Result);
                        responseMessage = "Gitloy: PushEvent received.";
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                responseMessage = $"Gitloy: Unsupported event received.";
                return UnprocessableEntity(responseMessage);
            }

            return Ok(responseMessage);
        }

        private Task GithubHandle(PingEvent data)
        {
            //Notice that user has setup repository correctly.
            return Task.CompletedTask;
        }

        private Task GithubHandle(PushEvent data)
        {
            var request = new WorkerJobRequest()
            {
                Id = 1337,
                FtpServer = new FtpServer()
                {
                    UserAccount = new FtpAccount()
                    {
                        Username = "gitloyer",
                        Password = "gitloypw123"
                    },
                    Hostname = "ftp.dacha204.com",
                    Port = 2121,
                    RootDirectory = "/"
                },
                GitRepository = new GitRepository()
                {
                    Branch = "master",
                    Url = data.Repository.CloneUrl
                },
            };

            var response = _communicator.Bus.Request<WorkerJobRequest, WorkerJobResponse>(request);

            _logger.LogInformation($"Request executed: {response.ResultStatus} => {response.ResultMessage}");

            return Task.CompletedTask;
        }
    }
}