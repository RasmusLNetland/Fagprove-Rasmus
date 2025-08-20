using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// Token Response
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets bearer token, used for authentication
        /// </summary>
        [JsonProperty( PropertyName = "token" )]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets expiry of Bearer token
        /// </summary>
        [JsonProperty( PropertyName = "expiresIn" )]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets refresh token, used to refresh Bearer Token without
        /// using credentials.
        /// </summary>
        [JsonProperty( PropertyName = "refreshToken" )]
        public string RefreshToken { get; set; }
    }
}