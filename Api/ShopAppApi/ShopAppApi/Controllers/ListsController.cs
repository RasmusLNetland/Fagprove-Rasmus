using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAppApi.BusinessLogic.Lists;
using ShopAppApi.Infrastructure.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopAppApi.Controllers;

/// <summary>
/// Lists Controller
/// </summary>
[ApiVersion( "1" )]
[Route( "api/v{version:apiVersion}" )]
[ApiController]
public class ListsController : ControllerBase
{
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public ListsController( IMediator mediator )
    {
        _mediator = mediator ?? throw new ArgumentNullException( nameof(mediator) );
    }

    #endregion

    /// <summary>
    /// Creates a list, with items.
    /// </summary>
    /// <param name="listRequest">Login request</param>
    /// <param name="cancellationToken"></param>
    /// <returns>ListResponse</returns>
    [HttpPost]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists", Name = "CreateList" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "List Response", typeof(ListResponse) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> CreateList( [FromBody] ListCreationRequest listRequest,
                                                 CancellationToken cancellationToken = default )
    {
        ListResponse result =
            await _mediator.Send( new CreateListCommand()
            {
                Request = listRequest
            }, cancellationToken );

        return Ok( result );
    }

    /// <summary>
    /// Gets all lists for user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>ListResponse</returns>
    [HttpGet]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists", Name = "GetLists" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "List response (IEnumerable)", typeof(IEnumerable<ListResponse>) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> GetLists( CancellationToken cancellationToken = default )
    {
        IEnumerable<ListResponse> result =
            await _mediator.Send( new GetListsQuery(), cancellationToken );

        return Ok( result );
    }

    /// <summary>
    /// Gets items in a list
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>ItemResponse</returns>
    [HttpGet]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists/{id:int}", Name = "GetItemsForList" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "List response (IEnumerable)", typeof(IEnumerable<ListResponse>) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> GetItemsForList( [FromRoute] int id, CancellationToken cancellationToken = default )
    {
        IEnumerable<ItemResponse> result =
            await _mediator.Send( new GetItemsForListQuery()
            {
                ListId = id
            }, cancellationToken );

        return Ok( result );
    }

    /// <summary>
    /// Alters the status of items in a list
    /// </summary>
    /// <param name="itemStatuses"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists/items", Name = "SetItemsStatuses" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "" )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> SetItemsStatuses( [FromBody] Dictionary<int, bool> itemStatuses,
                                                       CancellationToken cancellationToken = default )
    {
        await _mediator.Send( new BatchMarkItemsCommand()
        {
            ItemStatuses = itemStatuses
        }, cancellationToken );

        return Ok();
    }

    /// <summary>
    /// Gets template lists
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>ListResponse</returns>
    [HttpGet]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists-templates", Name = "GetTemplateLists" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "Template lists response (IEnumerable)", typeof(IEnumerable<ListResponse>) )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> GetTemplateLists( CancellationToken cancellationToken = default )
    {
        IEnumerable<ListResponse> result =
            await _mediator.Send( new GetTemplateListsQuery(), cancellationToken );

        return Ok( result );
    }

    /// <summary>
    /// Updates a list.
    /// </summary>
    /// <param name="request">Update request</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists", Name = "UpdateList" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "" )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> UpdateList( [FromBody] UpdateListRequest request,
                                                 CancellationToken cancellationToken = default )
    {
        await _mediator.Send( new UpdateListCommand()
        {
            Request = request
        }, cancellationToken );

        return Ok();
    }

    /// <summary>
    /// Archives a list.
    /// </summary>
    /// <param name="id">List id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [MapToApiVersion( "1" )]
    [Authorize]
    [Route( "lists/{id:int}", Name = "ArchiveList" )]
    [Produces( "application/json" )]
    [SwaggerResponse( (int)HttpStatusCode.OK, "" )]
#if !DEBUG
    [SwaggerResponse( (int)HttpStatusCode.Unauthorized, "Unauthorized" )]
    [SwaggerResponse( (int)HttpStatusCode.NoContent, "No Content" )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, "Bad Request" )]
    [SwaggerResponse( (int)HttpStatusCode.InternalServerError, "Internal server error" )]
#endif
    public async Task<IActionResult> ArchiveList( [FromRoute] int id,
                                                  CancellationToken cancellationToken = default )
    {
        await _mediator.Send( new ArchiveListCommand
        {
            ListId = id
        }, cancellationToken );

        return Ok();
    }

    private readonly IMediator _mediator;
}