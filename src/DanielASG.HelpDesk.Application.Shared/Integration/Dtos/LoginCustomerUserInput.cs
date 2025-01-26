using Newtonsoft.Json;

namespace DanielASG.HelpDesk.Integration.Dtos
{
    public class LoginCustomerUserInput
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("tenancy_name")]
        public string TenancyName { get; set; }

        [JsonProperty("user")]
        public UserInput User { get; set; }
    }

    public class UserInput
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
