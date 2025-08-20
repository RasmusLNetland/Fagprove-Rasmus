using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists;

/// <summary>
/// Handler of CreateListCommand
/// </summary>
public class CreateListCommandHandler : IRequestHandler<CreateListCommand, ListResponse>
{
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="internalDataStorage"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CreateListCommandHandler( IInternalDataStorage internalDataStorage, IHttpContextAccessor httpContextAccessor,
                                     ILogger<CreateListCommandHandler> logger )
    {
        _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException( nameof(httpContextAccessor) );
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    #endregion

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ListResponse> Handle( CreateListCommand command, CancellationToken cancellationToken )
    {
        try
        {
            int userId = _httpContextAccessor.HttpContext!.GetUserId();
            if( userId <= 0 )
                throw new Exception( "Invalid userId" );

            ListCreationRequest? request = command.Request;
            if( request is null || !request.IsValid() )
                throw new BadRequestException( "ListCreationRequest is not valid" );
            ListResponse list =
                await _internalDataStorage.CreateListAsync( request.Name, request.Items, userId, cancellationToken );
            return list;
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    #region Private members

    private readonly IInternalDataStorage _internalDataStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CreateListCommandHandler> _logger;

    #endregion
}