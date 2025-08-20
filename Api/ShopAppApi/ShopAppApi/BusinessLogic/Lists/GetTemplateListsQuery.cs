using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// GetTemplateListsQuery
    /// </summary>
    public class GetTemplateListsQuery : IRequest<IEnumerable<ListResponse>>;
}
