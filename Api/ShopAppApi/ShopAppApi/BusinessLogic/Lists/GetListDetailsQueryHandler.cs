using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler for GetListDetailsQuery
    /// </summary>
    public class GetListDetailsQueryHandler : IRequestHandler<GetListDetailsQuery, ListDetailsResponse>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalDataStorage"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GetListDetailsQueryHandler( IInternalDataStorage internalDataStorage, ILogger<GetListDetailsQueryHandler> logger )
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
        public async Task<ListDetailsResponse> Handle( GetListDetailsQuery request, CancellationToken cancellationToken )
        {
            try
            {
                ListDetailsResponse details =
                    await _internalDataStorage.GetListDetailsAsync( request.ListId, cancellationToken: cancellationToken );

                return details;
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<GetListDetailsQueryHandler> _logger;

        #endregion
    }
}