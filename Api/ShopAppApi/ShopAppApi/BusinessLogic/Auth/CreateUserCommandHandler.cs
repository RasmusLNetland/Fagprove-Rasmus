using MediatR;
using ShopAppApi.Auxiliary;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;
using ShopAppApi.Infrastructure.Services;

namespace ShopAppApi.BusinessLogic.Auth;

/// <summary>
/// Handler of CreateUserCommand
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, TokenResponse>
{
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="internalDataStorage"></param>
    /// <param name="tokenService"></param>
    /// <param name="logger"></param>
    public CreateUserCommandHandler( IInternalDataStorage internalDataStorage, ITokenService tokenService,
                                     ILogger<CreateUserCommandHandler> logger )
    {
        _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
        _tokenService = tokenService ?? throw new ArgumentNullException( nameof(tokenService) );
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    #endregion

    /// <summary>
    /// Handler
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TokenResponse> Handle( CreateUserCommand command, CancellationToken cancellationToken )
    {
        try
        {
            if( command.Request is null || !command.Request.IsValid() )
                throw new BadRequestException( "Invalid RegistrationRequest" );

            RegistrationRequest request = command.Request;

            string passwordHash = CredentialsUtils.CreatePasswordHash( request.Password, out string passwordSalt );

            UserResponse userInfo =
                await _internalDataStorage.CreateUserAsync( request.Email, request.FullName, passwordHash, passwordSalt,
                                                            cancellationToken );

            string refreshToken = await _internalDataStorage.CreateRefreshTokenAsync( request.Email, cancellationToken );

            TokenResponse tokenResponse = _tokenService.ConstructTokenResponse( userInfo.FullName, userInfo.Id, refreshToken );

            return tokenResponse;
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    #region Private Memberes

    private readonly IInternalDataStorage _internalDataStorage;
    private readonly ITokenService _tokenService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    #endregion
}