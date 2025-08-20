using MediatR;
using ShopAppApi.Auxiliary;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;
using ShopAppApi.Infrastructure.Services;

namespace ShopAppApi.BusinessLogic.Auth;

/// <summary>
/// Handler of GetUserAuthenticatedQuery
/// </summary>
public class GetUserAuthenticatedQueryHandler : IRequestHandler<GetUserAuthenticatedQuery, TokenResponse>
{
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="internalDataStorage"></param>
    /// <param name="tokenService"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GetUserAuthenticatedQueryHandler( IInternalDataStorage internalDataStorage, ITokenService tokenService,
                                             ILogger<GetUserAuthenticatedQueryHandler> logger )
    {
        _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
        _tokenService = tokenService ?? throw new ArgumentNullException( nameof(tokenService) );
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    #endregion

    /// <summary>
    /// Handler
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<TokenResponse> Handle( GetUserAuthenticatedQuery query, CancellationToken cancellationToken )
    {
        try
        {
            if( query.Request is null || !query.Request.IsValid() )
                throw new BadRequestException( "Invalid Login request" );

            LoginRequest request = query.Request;
            UserResponse? userInfo;
            string? refreshToken;

            switch( request.AuthType )
            {
                case nameof(AuthTypeEnum.Password):
                    userInfo = await ValidateUserInfo( request, cancellationToken );
                    refreshToken =
                        await _internalDataStorage.CreateRefreshTokenAsync( request.Email!,
                                                                            cancellationToken:
                                                                            cancellationToken ); // Email is checked for null in Request.IsValid()
                    break;
                case nameof(AuthTypeEnum.RefreshToken):
                    (refreshToken, userInfo) =
                        await RenewRefreshToken( request.RefreshToken!,
                                                 cancellationToken ); // RefreshToken is checked for null in Request.IsValid()
                    break;
                default:
                    throw new BadRequestException( "Invalid AuthType, valid types are (Password, RefreshToken)" );
            }

            TokenResponse tokenResponse = _tokenService.ConstructTokenResponse( userInfo.FullName, userInfo.Id, refreshToken );

            return tokenResponse;
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    #region Private members

    private async Task<UserResponse> ValidateUserInfo( LoginRequest request, CancellationToken cancellationToken = default )
    {
        UserResponse? userInfo =
            await _internalDataStorage.GetUserAuthenticationInfoAsync( request.Email!,
                                                                       cancellationToken ); // Email is checked for null in Request.IsValid()
        if( userInfo is null )
            throw new NotFoundException( $"User with email {request.Email} is not found" );

        if( string.CompareOrdinal( userInfo.PasswordHash,
                                   CredentialsUtils.CreatePasswordHash( request.Password!, userInfo.PasswordSalt ) ) !=
            0 ) // Password is checked for null in Request.IsValid()
            throw new BadRequestException( $"Invalid password for email {request.Email}" );

        return userInfo;
    }

    private async Task<(string, UserResponse)> RenewRefreshToken( string token, CancellationToken cancellationToken = default )
    {
        (string, UserResponse)? renewResult = await _internalDataStorage.RenewRefreshTokenAsync( token, cancellationToken );
        if( !renewResult.HasValue )
            throw new UnprocessableEntityException( "Refresh token was not able to be generated" );

        return renewResult.Value;
    }

    private readonly IInternalDataStorage _internalDataStorage;
    private readonly ITokenService _tokenService;
    private readonly ILogger<GetUserAuthenticatedQueryHandler> _logger;

    #endregion
}