namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// ListDetailsResponse
/// </summary>
public class ListDetailsResponse
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Items
    /// </summary>
    public IEnumerable<ItemResponse> Items { get; set; }

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        ListDetailsResponse? response = obj as ListDetailsResponse;
        if( response is null )
            return false;

        return Name == response.Name
               && Items == response.Items;
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
}