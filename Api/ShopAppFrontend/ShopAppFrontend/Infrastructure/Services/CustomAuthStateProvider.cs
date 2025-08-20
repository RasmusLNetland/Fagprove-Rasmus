using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ShopAppFrontend.Infrastructure.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public CustomAuthStateProvider( ProtectedLocalStorage localStorage )
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = (await _localStorage.GetAsync<string>( "authToken" )).Value;
            ClaimsIdentity identity = GetClaimsIdentity( token );
            ClaimsPrincipal user = new(identity);
            return new AuthenticationState( user );
        }

        public async Task MarkUserAsAuthenticated( string token )
        {
            ClaimsIdentity identity = GetClaimsIdentity( token );
            ClaimsPrincipal user = new(identity);
            NotifyAuthenticationStateChanged( Task.FromResult( new AuthenticationState( user ) ) );
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.DeleteAsync( "authToken" );
            await _localStorage.DeleteAsync( "refreshToken" );
            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal( identity );
            NotifyAuthenticationStateChanged( Task.FromResult( new AuthenticationState( user ) ) );
        }

        private ClaimsIdentity GetClaimsIdentity( string token )
        {
            ClaimsIdentity identity;
            if( !string.IsNullOrEmpty( token ) )
            {
                JwtSecurityTokenHandler handler = new();
                JwtSecurityToken? jwtToken = handler.ReadJwtToken( token );

                // Convert JWT claims into ClaimsIdentity
                identity = new ClaimsIdentity( jwtToken.Claims, "Bearer" );
            }
            else
            {
                identity = new ClaimsIdentity(); // not authenticated
            }

            return identity;
        }

        private readonly ProtectedLocalStorage _localStorage;
    }
}