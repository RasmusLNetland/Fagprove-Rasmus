using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists;

/// <summary>
/// GetListsQuery
/// </summary>
public class GetListsQuery : IRequest<IEnumerable<ListResponse>>;