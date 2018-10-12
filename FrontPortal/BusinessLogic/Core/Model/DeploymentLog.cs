using System;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Model
{
    public class DeploymentLog : Model
    {
        public DateTime DateTime { get; set; }
        public ResultStatus Status { get; set; }
        public string ResultMessage { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        
        public virtual Deployment Deployment { get; set; }
        
        public DeploymentLog()
        {
            DateTime = DateTime.Now;
            Status = ResultStatus.Unknown;
            ResultMessage = string.Empty;
            Description = string.Empty;
            EventType = string.Empty;
        }
    }
}