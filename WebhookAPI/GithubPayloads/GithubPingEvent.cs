using Gitloy.Services.WebhookAPI.GithubPayloads.Common;
using Newtonsoft.Json;

namespace Gitloy.Services.WebhookAPI.GithubPayloads
{
    public class GithubPingEvent
    {
        [JsonProperty("zen")]
        public string Zen { get; set; }

        [JsonProperty("hook_id")]
        public int HookId { get; set; }

        [JsonProperty("hook")]
        public Hook Hook { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public Sender Sender { get; set; }
    }
}