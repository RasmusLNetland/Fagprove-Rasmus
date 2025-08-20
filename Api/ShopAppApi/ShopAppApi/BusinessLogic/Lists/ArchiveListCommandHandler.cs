using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.DataStorage;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler of ArchiveListCommand
    /// </summary>
    public class ArchiveListCommandHandler : IRequestHandler<ArchiveListCommand>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveListCommandHandler( IInternalDataStorage internalDataStorage, ILogger<ArchiveListCommandHandler> logger )
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle( ArchiveListCommand request, CancellationToken cancellationToken )
        {
            try
            {
                if( request.ListId <= 0 )
                    throw new BadRequestException( "Invalid List Id" );

                await _internalDataStorage.DeleteListAsync( request.ListId, cancellationToken );
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<ArchiveListCommandHandler> _logger;

        #endregion
    }
}