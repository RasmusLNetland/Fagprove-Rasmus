using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;

namespace ShopAppFrontend.Auxiliary
{
    public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public Task HandleAsync( RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
                                 PolicyAuthorizationResult authorizeResult )
        {
            return next( context );
        }
    }
}