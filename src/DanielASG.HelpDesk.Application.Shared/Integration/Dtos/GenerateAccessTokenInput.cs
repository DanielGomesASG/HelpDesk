using Newtonsoft.Json;

namespace DanielASG.HelpDesk.Integration.Dtos
{
    public class GenerateAccessTokenInput
    {
        [JsonProperty("tenancy_name")]
        public string TenancyName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
