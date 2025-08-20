using ShopAppApi.Infrastructure.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Infrastructure.DataStorage;

public interface IInternalDataStorage
{
    Task<UserResponse?> GetUserAuthenticationInfoAsync( string email, CancellationToken cancellationToken = default );

    Task<UserResponse> CreateUserAsync( string email, string fullName, string passwordHash, string passwordSalt,
                                        CancellationToken cancellationToken = default );

    Task<string> CreateRefreshTokenAsync( string email, CancellationToken cancellationToken = default );
    Task<(string, UserResponse)> RenewRefreshTokenAsync( string token, CancellationToken cancellationToken = default );

    Task<ListResponse> CreateListAsync( string listName, IEnumerable<ItemCreationRequest> listItems, int userId,
                                        CancellationToken cancellationToken = default );

    Task<IEnumerable<ListResponse>> GetListsByUserIdAsync( int userId, CancellationToken cancellationToken = default );
    Task<IEnumerable<ItemResponse>> GetItemsForListAsync( int listId, CancellationToken cancellationToken = default );

    Task BatchMarkItemsAsCheckedAsync( Dictionary<int, bool> itemStatuses,
                                       CancellationToken cancellationToken = default );

    Task<IEnumerable<ListResponse>> GetTemplateListsAsync( CancellationToken cancellationToken = default );
    Task UpdateListAsync( UpdateListRequest request, CancellationToken cancellationToken = default );
    Task DeleteListAsync( int id, CancellationToken cancellationToken = default );
}