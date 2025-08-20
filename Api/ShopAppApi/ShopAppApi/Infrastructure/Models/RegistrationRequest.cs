namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Registration request
/// </summary>
public class RegistrationRequest
{
    /// <summary>
    /// Email of user to be registered.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Full name of user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Password for user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        RegistrationRequest? request = obj as RegistrationRequest;
        if( request is null )
            return false;

        return string.CompareOrdinal( Email, request.Email ) == 0
               && string.CompareOrdinal( FullName, request.FullName ) == 0
               && string.CompareOrdinal( Password, request.Password ) == 0;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Email.GetHashCode() ^ FullName.GetHashCode() ^ Password.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// Validation of Registration Request
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty( Email ) && !string.IsNullOrEmpty( FullName ) && !string.IsNullOrEmpty( Password );
    }
}