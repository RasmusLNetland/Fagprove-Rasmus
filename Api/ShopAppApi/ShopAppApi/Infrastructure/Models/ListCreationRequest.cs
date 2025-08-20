namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Creation request for shopping lists
/// </summary>
public class ListCreationRequest
{
    /// <summary>
    /// Name of shopping list
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// List of shopping items
    /// </summary>
    public IEnumerable<ItemCreationRequest> Items { get; set; } = new List<ItemCreationRequest>();

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        ListCreationRequest? request = obj as ListCreationRequest;
        if( request is null )
            return false;

        return Name == request.Name
               && Items == request.Items;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Name.GetHashCode()
               ^ Items.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// Validation
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name) && Items.Any();
    }
}