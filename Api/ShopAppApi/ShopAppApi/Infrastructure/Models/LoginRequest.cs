using ShopAppApi.Infrastructure.Common;

namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Login Request
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Type of authentication request. types "password" and "refresh_token" are valid.
    /// </summary>
    public string AuthType { get; set; } = string.Empty;

    /// <summary>
    /// Email of user. Has to be provided with auth_type "password".
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Password. Has to be provided with auth_type "password".
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Refresh token. Has to be provided with auth_type "refresh_token".
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        LoginRequest? request = obj as LoginRequest;
        if( request is null )
            return false;

        return string.CompareOrdinal( AuthType, request.AuthType ) == 0
               && (Email is null && request.Email is null || Email is not null && string.CompareOrdinal( Email, request.Email ) == 0)
               && (Password is null && request.Password is null ||
                   Password is not null && string.CompareOrdinal( Password, request.Password ) == 0)
               && (RefreshToken is null && request.RefreshToken is null ||
                   RefreshToken is not null && string.CompareOrdinal( RefreshToken, request.RefreshToken ) == 0);
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return AuthType.GetHashCode()
               ^ (Email?.GetHashCode() ?? 0)
               ^ (Password?.GetHashCode() ?? 0)
               ^ (RefreshToken?.GetHashCode() ?? 0);
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// Validation of Registration Request
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return Enum.TryParse<AuthTypeEnum>(AuthType, out AuthTypeEnum type)
               && (type == AuthTypeEnum.Password && !string.IsNullOrEmpty( Email ) && !string.IsNullOrEmpty( Password ) &&
                   string.IsNullOrEmpty( RefreshToken ))
               || (type == AuthTypeEnum.RefreshToken && string.IsNullOrEmpty( Email ) && string.IsNullOrEmpty( Password ) &&
                   !string.IsNullOrEmpty( RefreshToken ));
    }
}