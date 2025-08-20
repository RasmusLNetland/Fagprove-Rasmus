namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// User Response
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Id of User in database
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Email of user
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Password hashed
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Password salt
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        UserResponse? response = obj as UserResponse;
        if( response is null )
            return false;

        return Id == response.Id
               && string.CompareOrdinal( Email, response.Email ) == 0
               && string.CompareOrdinal( FullName, response.FullName ) == 0
               && string.CompareOrdinal( PasswordHash, response.PasswordHash ) == 0
               && string.CompareOrdinal( PasswordSalt, response.PasswordSalt ) == 0;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Id.GetHashCode()
               ^ Email.GetHashCode()
               ^ FullName.GetHashCode()
               ^ PasswordHash.GetHashCode()
               ^ PasswordSalt.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}