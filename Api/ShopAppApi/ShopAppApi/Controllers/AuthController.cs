using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopAppApi.BusinessLogic.Auth;
using ShopAppApi.Infrastructure.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopAppApi.Controllers;

/// <summary>
/// Auth Controller
/// </summary>
[ApiVersion( "1" )]
[Route( "api/v{version:apiVersion}" )]
[ApiController]
public class AuthController : ControllerBase
{
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AuthController( IMediator mediator )
    {
        _mediator = mediator ?? throw new ArgumentNullException( nameof(mediator) );
    }

    #endregion

    /// <summary>
    /// Returns the user's auth token, by providing login details, or refresh token.
    /// </summary>
    /// <param name="loginRequest">Login request</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Token Response</returns>
    [HttpPost]
    [MapToApiVersion( "1" )]
    [Route( "auth/login", Name = "GetUserAuthenticated" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "Token Response", typeof(TokenResponse) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> GetUserAuthenticated( [FromForm] LoginRequest loginRequest,
                                                           CancellationToken cancellationToken = default )
    {
        TokenResponse result =
            await _mediator.Send( new GetUserAuthenticatedQuery
            {
                Request = loginRequest
            }, cancellationToken );

        return Ok( result );
    }

    /// <summary>
    /// Creates a user, and returns token response.
    /// </summary>
    /// <param name="registrationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Token Response</returns>
    [HttpPost]
    [MapToApiVersion( "1" )]
    [Route( "auth/register", Name = "CreateUser" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "Token Response", typeof(TokenResponse) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> CreateUser( [FromForm] RegistrationRequest registrationRequest,
                                                 CancellationToken cancellationToken = default )
    {
        TokenResponse result =
            await _mediator.Send( new CreateUserCommand()
            {
                Request = registrationRequest
            }, cancellationToken );

        return Ok( result );
    }

    private readonly IMediator _mediator;
}