using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gitloy.BuildingBlocks.Messages.Data;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.Services.Common.Communicator;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers;
using Gitloy.Services.WebhookAPI.GithubPayloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.WebhookAPI.Controllers
{
    [ApiController]
    [Route("api/webhooks/incoming/github")]
    public class GitHubController : ControllerBase
    {
        private readonly ILogger<GitHubController> _logger;
        private readonly ICommunicator _communicator;
        private readonly IServiceProvider _serviceProvider;

        public GitHubController(
            ILogger<GitHubController> logger, 
            ICommunicator communicator,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _communicator = communicator;
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            string responseMessage;

            try
            {
                var payload = GithubPayloadExtractor.Extract(Request);

                switch (payload.Event)
                {
                    case "push":
                        await GithubHandle((GithubPushEvent) payload.Result);
                        responseMessage = "Gitloy: GithubPushEvent received.";
                        break;
                    case "ping":
                        await GithubHandle((GithubPingEvent) payload.Result);
                        responseMessage = "Gitloy: GithubPingEvent received.";
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception e)
            {
                responseMessage = $"Gitloy: Unsupported event received. \n{e.ToString()}" ;
                return UnprocessableEntity(responseMessage);
            }

            return Ok(responseMessage);
        }

        private Task GithubHandle(GithubPingEvent data)
        {
            var handler = _serviceProvider.GetService<IGitEventHandler<GithubPingEvent>>();
            handler.HandleAsync(data);
            
            return Task.CompletedTask;
        }

        private Task GithubHandle(GithubPushEvent data)
        {
            var handler = _serviceProvider.GetService<IGitEventHandler<GithubPushEvent>>();
            handler.HandleAsync(data);
            
            return Task.CompletedTask;
        }
    }
}