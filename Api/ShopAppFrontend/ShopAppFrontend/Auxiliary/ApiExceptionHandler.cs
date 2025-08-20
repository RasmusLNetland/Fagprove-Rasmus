using System.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ShopAppFrontend.Auxiliary;

public class ApiExceptionHandler : DelegatingHandler
{
    public ApiExceptionHandler( ErrorNotifier errorNotifier, ILogger<ApiExceptionHandler> logger )
    {
        _errorNotifier = errorNotifier ?? throw new ArgumentNullException( nameof(errorNotifier) );
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
    {
        HttpResponseMessage response;

        try
        {
            response = await base.SendAsync( request, cancellationToken );

            if( !response.IsSuccessStatusCode )
            {
                HttpStatusCode statusCode = response.StatusCode;
                string rawContent = await response.Content.ReadAsStringAsync( cancellationToken );

                if( statusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound )
                {
                    try
                    {
                        // Try to parse
                        ApiErrorResponse? error = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>( rawContent,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            } );

                        if( error != null && !string.IsNullOrWhiteSpace( error.Message ) )
                            _errorNotifier.NotifyError( error.Message );
                        else
                            throw new Exception(); // fallback in catch
                    }
                    catch
                    {
                        _errorNotifier.NotifyError( $"Bad Request: {rawContent}" );
                    }
                }
                else
                {
                    string msg = $"Request failed: {(int)response.StatusCode} {response.ReasonPhrase}";
                    _errorNotifier.NotifyError( msg );
                }
            }
        }
        catch(Exception ex)
        {
            const string message = "Noe gikk galt, vennligst prøv igjen senere.";
            _errorNotifier.NotifyError( message );
            _logger.LogError( ex, "Unhandled HttpClient exception" );
            throw;
        }

        return response;
    }

    private readonly ErrorNotifier _errorNotifier;
    private readonly ILogger<ApiExceptionHandler> _logger;
}

internal class ApiErrorResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}