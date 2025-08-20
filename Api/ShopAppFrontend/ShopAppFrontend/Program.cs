using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ShopAppFrontend.Auxiliary;
using ShopAppFrontend.Components;
using ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi;
using ShopAppFrontend.Infrastructure.Services;

namespace ShopAppFrontend
{
    public class Program
    {
        public static void Main( string[] args )
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder( args );

            // Add services to the container.
            builder.Services.AddRazorComponents()
                   .AddInteractiveServerComponents();

            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();

            builder.Services.AddAuthenticationCore();
            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddOutputCache();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<ApiExceptionHandler>();
            builder.Services.AddSingleton<ErrorNotifier>();
            builder.Services.AddScoped<ListState>();

            builder.Services
                   .AddHttpClient(
                       "ApiClient", client => { client.BaseAddress = new Uri( "https://localhost:7295/" ); } )
                   .AddHttpMessageHandler<ApiExceptionHandler>();
            builder.Services.AddScoped<IShopAppApiClient>(
                s => new ShopAppApiClient( s.GetRequiredService<IHttpClientFactory>(), s.GetRequiredService<ProtectedLocalStorage>() ) );

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if( !app.Environment.IsDevelopment() )
            {
                app.UseExceptionHandler( "/Error" );
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorComponents<App>()
               .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}