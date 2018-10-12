using System;

namespace Gitloy.Services.FrontPortal.ViewModels
{
    public class WebhookParamsViewModel
    {
        public Guid DeploymentGuid { get; set; }
        public string CreateWebhookURL { get; set; } = string.Empty;
        public string PayloadURL { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;


    }
}