using ShopAppApi.Infrastructure.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Infrastructure.Services;

public interface ITokenService
{
    TokenResponse ConstructTokenResponse( string name, int userId, string? refreshToken );
}