namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Handlers
{
    public class WebhookAPIOptions
    {
        public string Domain { get; set; }
        public string HookRoute { get; set; }

        public WebhookAPIOptions()
        {
            Domain = "localhost.default";
            HookRoute = "HookRoute.default";
        }
    }
}