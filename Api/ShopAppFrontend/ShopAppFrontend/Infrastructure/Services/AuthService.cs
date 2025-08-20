using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi;
using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models;
using ShopAppFrontend.Infrastructure.ViewModels;

namespace ShopAppFrontend.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IShopAppApiClient _shopAppApi;
    private readonly ProtectedLocalStorage _localStorage;

    public AuthService( IShopAppApiClient shopAppApi, ProtectedLocalStorage localStorage )
    {
        _shopAppApi = shopAppApi;
        _localStorage = localStorage;
    }

    public async Task<TokenResponse?> LoginAsync( LoginViewModel request, CancellationToken cancellationToken = default )
    {
        TokenResponse? result =
            await _shopAppApi.LoginWithPasswordAsync( request.Email, request.Password );
        if( result is null )
            return null;

        string? token = result.Token;
        string? refreshToken = result.RefreshToken;
        if( !string.IsNullOrEmpty( token ) && !string.IsNullOrEmpty( refreshToken ) ) await SaveTokens( token, refreshToken );

        return result;
    }

    public async Task<TokenResponse?> RegisterAsync( RegistrationRequest request, CancellationToken cancellation = default )
    {
        TokenResponse? result =
            await _shopAppApi.RegisterAsync( request.Email, request.FullName, request.Password );
        if( result is null )
            return null;

        string? token = result.Token;
        string? refreshToken = result.RefreshToken;
        if( !string.IsNullOrEmpty( token ) && !string.IsNullOrEmpty( refreshToken ) ) await SaveTokens( token, refreshToken );

        return result;
    }

    public async Task<TokenResponse?> TryRefreshTokenAsync( CancellationToken cancellationToken = default )
    {
        ProtectedBrowserStorageResult<string> refreshToken = await _localStorage.GetAsync<string>( "refreshToken" );
        if( refreshToken.Value is null )
            return null;

        TokenResponse? result = await _shopAppApi.LoginWithRefreshTokenAsync( refreshToken.Value );
        if( result is null )
            return null;

        string? token = result.Token;
        string? newRefreshToken = result.RefreshToken;
        if( !string.IsNullOrEmpty( token ) && !string.IsNullOrEmpty( newRefreshToken ) )
            await SaveTokens( token, newRefreshToken );

        return result;
    }

    public async Task LogoutAsync( CancellationToken cancellationToken = default )
    {
        await _localStorage.DeleteAsync( "authToken" );
        await _localStorage.DeleteAsync( "refreshToken" );
    }

    private async Task SaveTokens( string token, string refreshToken )
    {
        await _localStorage.SetAsync( "authToken", token );
        await _localStorage.SetAsync( "refreshToken", refreshToken );
    }
}