using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Auth;

/// <summary>
/// Get User Authenticated
/// </summary>
public class GetUserAuthenticatedQuery : IRequest<TokenResponse>

{
    /// <summary>
    /// Login request
    /// </summary>
    public LoginRequest? Request { get; set; }

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        GetUserAuthenticatedQuery? query = obj as GetUserAuthenticatedQuery;
        if( query is null )
            return false;

        return (Request is null && query.Request is null) || (Request is not null && Request == query.Request);
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Request?.GetHashCode() ?? 0;
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}