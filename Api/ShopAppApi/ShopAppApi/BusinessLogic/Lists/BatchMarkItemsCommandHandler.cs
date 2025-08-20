using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.DataStorage;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler of BatchMarkItemsCommand
    /// </summary>
    public class BatchMarkItemsCommandHandler : IRequestHandler<BatchMarkItemsCommand>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalDataStorage"></param>
        /// <param name="logger"></param>
        public BatchMarkItemsCommandHandler( IInternalDataStorage internalDataStorage, ILogger<BatchMarkItemsCommandHandler> logger )
        {
            _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
            _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
        }

        #endregion

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle( BatchMarkItemsCommand request, CancellationToken cancellationToken )
        {
            try
            {
                if( request.ItemStatuses.Count > 0 )
                    await _internalDataStorage.BatchMarkItemsAsCheckedAsync( request.ItemStatuses, cancellationToken );
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<BatchMarkItemsCommandHandler> _logger;

        #endregion
    }
}