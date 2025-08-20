using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler of GetListsQuery
    /// </summary>
    public class GetListsQueryHandler : IRequestHandler<GetListsQuery, IEnumerable<ListResponse>>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalDataStorage"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public GetListsQueryHandler( IInternalDataStorage internalDataStorage, IHttpContextAccessor httpContextAccessor,
                                     ILogger<GetListsQueryHandler> logger )
        {
            _internalDataStorage = internalDataStorage;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<ListResponse>> Handle( GetListsQuery request, CancellationToken cancellationToken )
        {
            try
            {
                int userId = _httpContextAccessor.HttpContext!.GetUserId();
                if( userId <= 0 )
                    throw new Exception("User is unauthorized");

                IEnumerable<ListResponse> lists = await _internalDataStorage.GetListsByUserIdAsync( userId, cancellationToken );
                return lists;
            }
            catch (Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetListsQueryHandler> _logger;

        #endregion
    }
}