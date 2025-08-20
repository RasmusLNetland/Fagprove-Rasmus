namespace ShopAppApi.Infrastructure.Models
{
    /// <summary>
    /// Token Response
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Bearer token, used for authentication
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Expiry of Bearer token
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Refresh token, used to refresh Bearer Token without using credentials.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            TokenResponse? response = obj as TokenResponse;
            if( response is null )
                return false;

            return string.CompareOrdinal( Token, response.Token ) == 0
                   && ExpiresIn == response.ExpiresIn
                   && (RefreshToken is null && response.RefreshToken is null ||
                       RefreshToken is not null && string.CompareOrdinal( RefreshToken, response.RefreshToken ) == 0);
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Token.GetHashCode()
                   ^ ExpiresIn.GetHashCode()
                   ^ (RefreshToken?.GetHashCode() ?? 0);
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}