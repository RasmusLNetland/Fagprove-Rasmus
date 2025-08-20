using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// Registration Request
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Full Name
        /// </summary>
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; } = string.Empty;
    }
}
