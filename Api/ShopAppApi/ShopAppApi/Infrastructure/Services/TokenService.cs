using Microsoft.IdentityModel.Tokens;
using ShopAppApi.Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using ShopAppApi.Common.Extensions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Infrastructure.Services;

public class TokenService : ITokenService
{
    public TokenService( string fullKey, ILogger<TokenService> logger )
    {
        if( string.IsNullOrWhiteSpace( fullKey ) )
            throw new ArgumentNullException( nameof(fullKey) );
        _fullKey = fullKey;
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    public TokenResponse ConstructTokenResponse( string name, int userId, string? refreshToken ) // TOOD: don't use UserResponse here?
    {
        try
        {
            SecurityTokenDescriptor descriptor = CreateSecurityToken( name, userId );

            string tokenStr;
            using( RSA rsa = RSA.Create() )
            {
                rsa.FromXmlString( _fullKey );
                SecurityKey key = new RsaSecurityKey( rsa )
                {
                    CryptoProviderFactory = new CryptoProviderFactory
                    {
                        CacheSignatureProviders = false
                    }
                };
                descriptor.SigningCredentials = new SigningCredentials( key, SecurityAlgorithms.RsaSha512 );
                JwtSecurityTokenHandler tokenHandler = new();
                SecurityToken token = tokenHandler.CreateToken( descriptor );
                tokenStr = tokenHandler.WriteToken( token );
            }

            return new TokenResponse
            {
                Token = tokenStr,
                ExpiresIn = descriptor.Expires.HasValue
                    ? (int)(descriptor.Expires.Value - DateTime.UtcNow).TotalSeconds
                    : 0,
                RefreshToken = refreshToken
            };
        }
        catch(Exception ex)
        {
            ex.ProcessAndLogException( _logger );
            throw;
        }
    }

    private SecurityTokenDescriptor CreateSecurityToken( string name, int userId )
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity( new[]
            {
                new Claim( "sub", userId.ToString() ),
                new Claim( "fullname", name )
            } ),
            Issuer = "ShopAppIssuer",
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours( 1 )
        };
        return tokenDescriptor;
    }

    private readonly string _fullKey;
    private readonly ILogger<TokenService> _logger;
}