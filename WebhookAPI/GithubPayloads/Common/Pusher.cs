using Newtonsoft.Json;

namespace Gitloy.Services.WebhookAPI.GithubPayloads.Common
{
    public class Pusher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}