using System;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;

namespace Gitloy.Services.FrontPortal.ViewModels.DeploymentLog
{
    public class DeploymentLogViewModel
    {
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateTime { get; set; }
        public ResultStatus Status { get; set; }
        public string ResultMessage { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public DeploymentLogViewModel()
        {
        }

        public DeploymentLogViewModel(BusinessLogic.Core.Model.DeploymentLog log)
        {
            Guid = log.Guid;
            DateTime = log.DateTime;
            ResultMessage = log.ResultMessage;
            CreatedAt = log.CreatedAt;
            Status = log.Status;
            Description = log.Description;
            Type = log.EventType;
        }
    }
}