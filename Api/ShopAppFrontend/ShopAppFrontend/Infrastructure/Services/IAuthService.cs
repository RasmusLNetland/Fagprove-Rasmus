using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models;
using ShopAppFrontend.Infrastructure.ViewModels;

namespace ShopAppFrontend.Infrastructure.Services;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync( LoginViewModel request, CancellationToken cancellationToken = default );
    Task<TokenResponse?> TryRefreshTokenAsync( CancellationToken cancellationToken = default );
    Task LogoutAsync( CancellationToken cancellationToken = default );
    Task<TokenResponse?> RegisterAsync( RegistrationRequest request, CancellationToken cancellation = default );
}