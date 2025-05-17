using Newtonsoft.Json;

namespace RunGroopWebApp.Helpers
{
    public class IPInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonProperty("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("loc")]
        public string Location { get; set; } = string.Empty;

        [JsonProperty("org")]
        public string Org { get; set; } = string.Empty;

        [JsonProperty("postal")]
        public string Postal { get; set; } = string.Empty;

        [JsonProperty("timezone")]
        public string Timezone { get; set; } = string.Empty;
    }
}
