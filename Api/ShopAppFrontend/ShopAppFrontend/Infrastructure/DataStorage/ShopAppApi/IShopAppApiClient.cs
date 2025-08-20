using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi;

public interface IShopAppApiClient
{
    Task<TokenResponse?> LoginWithPasswordAsync( string email, string password );
    Task<TokenResponse?> LoginWithRefreshTokenAsync( string refreshToken );
    Task<IEnumerable<ListResponse>> GetListsForUserAsync( CancellationToken cancellationToken = default );
    Task<ListDetailsResponse> GetListDetailsAsync( int listId, CancellationToken cancellationToken = default );

    Task<ListResponse?> CreateListAsync( string name, IEnumerable<ItemCreationRequest> items,
                                         CancellationToken cancellationToken = default );

    Task MarkItemsCheckedAsync( Dictionary<int, bool> itemStatuses, CancellationToken cancellationToken = default );
    Task<IEnumerable<ListResponse>> GetTemplateLists( CancellationToken cancellationToken = default );
    Task<TokenResponse?> RegisterAsync( string email, string fullName, string password );

    Task UpdateListAsync( UpdateListRequest request,
                          CancellationToken cancellationToken = default );

    Task ArchiveListAsync( int listId,
                           CancellationToken cancellationToken = default );
}