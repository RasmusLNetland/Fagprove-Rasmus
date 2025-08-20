using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler of UpdateListCommand
    /// </summary>
    public class UpdateListCommandHandler : IRequestHandler<UpdateListCommand>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalDataStorage"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UpdateListCommandHandler( IInternalDataStorage internalDataStorage, ILogger<UpdateListCommandHandler> logger )
        {
            _internalDataStorage = internalDataStorage ?? throw new ArgumentNullException( nameof(internalDataStorage) );
            _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
        }

        #endregion

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle( UpdateListCommand command, CancellationToken cancellationToken )
        {
            try
            {
                UpdateListRequest? request = command.Request;
                if( request is null || !request.IsValid() )
                    throw new BadRequestException( "UpdateListRequest is invalid" );

                await _internalDataStorage.UpdateListAsync( request, cancellationToken );
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<UpdateListCommandHandler> _logger;

        #endregion
    }
}