using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Common.Extensions;

public static class HttpContextExtensions
{
    public static int GetUserId( this HttpContext httpContext )
    {
        string token = httpContext.GetAccessToken();

        if( string.IsNullOrWhiteSpace( token ) )
            return -1;

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken? jwtToken = handler.ReadJwtToken( token );

        return int.Parse( jwtToken.Subject, CultureInfo.InvariantCulture );
    }

    public static string GetAccessToken( this HttpContext httpContext )
    {
        return httpContext.TryGetAccessToken( out string token ) ? token : string.Empty;
    }

    public static bool TryGetAccessToken( this HttpContext httpContext, out string token )
    {
        token = httpContext.Request.Headers["Authorization"].ToString()
                           .Replace( "Bearer ", "", StringComparison.OrdinalIgnoreCase );

        return !string.IsNullOrWhiteSpace( token );
    }
}