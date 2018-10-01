using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gitloy.Services.WebhookAPI.GithubPayloads.Common
{
    public class Hook
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("events")]
        public IList<string> Events { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("test_url")]
        public string TestUrl { get; set; }

        [JsonProperty("ping_url")]
        public string PingUrl { get; set; }

        [JsonProperty("last_response")]
        public LastResponse LastResponse { get; set; }
    }
}