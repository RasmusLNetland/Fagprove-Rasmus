using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi;

public class ShopAppApiClient : IShopAppApiClient
{
    public ShopAppApiClient( IHttpClientFactory httpClientFactory, ProtectedLocalStorage localStorage )
    {
        _httpClient = httpClientFactory.CreateClient( "ApiClient" );
        _localStorage = localStorage;
    }

    public async Task<TokenResponse?> LoginWithPasswordAsync( string email, string password )
    {
        MultipartFormDataContent form = new()
        {
            { new StringContent( "Password" ), "authType" },
            { new StringContent( email ), "email" },
            { new StringContent( password ), "password" }
        };

        HttpResponseMessage response = await _httpClient.PostAsync( "/api/v1/auth/login", form );

        if( !response.IsSuccessStatusCode )
            return null;

        string content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TokenResponse>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } );
    }

    public async Task<TokenResponse?> LoginWithRefreshTokenAsync( string refreshToken )
    {
        MultipartFormDataContent form = new()
        {
            { new StringContent( "RefreshToken" ), "authType" },
            { new StringContent( refreshToken ), "refreshToken" }
        };

        HttpResponseMessage response = await _httpClient.PostAsync( "/api/v1/auth/login", form );

        if( !response.IsSuccessStatusCode )
            return null;

        string content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TokenResponse>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } );
    }

    public async Task<TokenResponse?> RegisterAsync( string email, string fullName, string password )
    {
        MultipartFormDataContent form = new()
        {
            { new StringContent( email ), "email" },
            { new StringContent( fullName ), "fullName" },
            { new StringContent( password ), "password" }
        };

        HttpResponseMessage response = await _httpClient.PostAsync( "/api/v1/auth/register", form );

        if( !response.IsSuccessStatusCode )
            return null;

        string content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TokenResponse>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } );
    }

    public async Task<IEnumerable<ListResponse>> GetListsForUserAsync( CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );
        HttpResponseMessage response = await _httpClient.GetAsync( "api/v1/lists", cancellationToken );
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync( cancellationToken );
        return JsonSerializer.Deserialize<IEnumerable<ListResponse>>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } ) ?? Array.Empty<ListResponse>();
    }

    public async Task<IEnumerable<ListResponse>> GetTemplateLists( CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );
        HttpResponseMessage response = await _httpClient.GetAsync( "api/v1/lists-templates", cancellationToken );
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync( cancellationToken );
        return JsonSerializer.Deserialize<IEnumerable<ListResponse>>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } ) ?? Array.Empty<ListResponse>();
    }

    public async Task<ListDetailsResponse> GetListDetailsAsync( int listId, CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );
        HttpResponseMessage response = await _httpClient.GetAsync( $"api/v1/lists/{listId}", cancellationToken );
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync( cancellationToken );
        return JsonSerializer.Deserialize<ListDetailsResponse>( content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } ) ?? new();
    }

    public async Task<ListResponse?> CreateListAsync( string name, IEnumerable<ItemCreationRequest> items,
                                                      CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );

        var body = new
        {
            name,
            items
        };

        string json = JsonSerializer.Serialize( body );
        using StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync( "api/v1/lists", content, cancellationToken );
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync( cancellationToken );
        return JsonSerializer.Deserialize<ListResponse>( responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        } );
    }

    public async Task MarkItemsCheckedAsync( Dictionary<int, bool> itemStatuses, CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );

        string json = JsonSerializer.Serialize( itemStatuses );
        using StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync( "api/v1/lists/items", content, cancellationToken );
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateListAsync( UpdateListRequest request,
                                       CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );

        string json = JsonSerializer.Serialize( request );
        using StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync( "api/v1/lists", content, cancellationToken );
        response.EnsureSuccessStatusCode();
    }

    public async Task ArchiveListAsync( int listId,
                                        CancellationToken cancellationToken = default )
    {
        await AddAuthHeaderAsync( cancellationToken );

        HttpResponseMessage response = await _httpClient.DeleteAsync( $"api/v1/lists/{listId}", cancellationToken );
        response.EnsureSuccessStatusCode();
    }

    private async Task AddAuthHeaderAsync( CancellationToken cancellationToken = default )
    {
        string? token = (await _localStorage.GetAsync<string>( "authToken" )).Value;
        if( !string.IsNullOrWhiteSpace( token ) )
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", token );
        }
    }

    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;
}