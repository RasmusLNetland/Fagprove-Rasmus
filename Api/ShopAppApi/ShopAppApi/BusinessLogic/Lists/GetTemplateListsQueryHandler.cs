using MediatR;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// Handler of GetTemplateListsQuery
    /// </summary>
    public class GetTemplateListsQueryHandler : IRequestHandler<GetTemplateListsQuery, IEnumerable<ListResponse>>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public GetTemplateListsQueryHandler( IInternalDataStorage internalDataStorage, ILogger<GetTemplateListsQueryHandler> logger )
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
        public async Task<IEnumerable<ListResponse>> Handle( GetTemplateListsQuery request, CancellationToken cancellationToken )
        {
            try
            {
                IEnumerable<ListResponse> templates = await _internalDataStorage.GetTemplateListsAsync( cancellationToken );
                return templates;
            }
            catch(Exception ex)
            {
                throw ex.ProcessAndLogException( _logger );
            }
        }

        #region Private members

        private readonly IInternalDataStorage _internalDataStorage;
        private readonly ILogger<GetTemplateListsQueryHandler> _logger;

        #endregion
    }
}