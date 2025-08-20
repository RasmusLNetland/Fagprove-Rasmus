using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler for GetItemsForListQuery
    /// </summary>
    public class GetItemsForListQueryHandler : IRequestHandler<GetItemsForListQuery, IEnumerable<ItemResponse>>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalDataStorage"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GetItemsForListQueryHandler( IInternalDataStorage internalDataStorage, ILogger<GetItemsForListQueryHandler> logger )
        {
            _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
            _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
        }

        #endregion

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ItemResponse>> Handle( GetItemsForListQuery request, CancellationToken cancellationToken )
        {
            try
            {
                IEnumerable<ItemResponse> items =
                    await _internalDataStorage.GetItemsForListAsync( request.ListId, cancellationToken: cancellationToken );

                return items;
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<GetItemsForListQueryHandler> _logger;

        #endregion
    }
}