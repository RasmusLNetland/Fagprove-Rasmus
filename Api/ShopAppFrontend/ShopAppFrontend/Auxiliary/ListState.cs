using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi;
using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models;

namespace ShopAppFrontend.Auxiliary
{
    public class ListState
    {
        public List<ListResponse> Lists { get; private set; } = new();

        public event Action? OnChange;

        public ListState( IShopAppApiClient apiClient )
        {
            _apiClient = apiClient;
        }

        public async Task RefreshAsync()
        {
            IEnumerable<ListResponse> result = await _apiClient.GetListsForUserAsync();
            Lists = result.ToList();
            OnChange?.Invoke();
        }

        private readonly IShopAppApiClient _apiClient;
    }
}