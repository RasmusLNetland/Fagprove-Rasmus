namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Response of ListItem
/// </summary>
public class ItemResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of parent List
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Name of Item
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Created On
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Checked On
    /// </summary>
    public DateTime? CheckedOn { get; set; }

    /// <summary>
    /// Count of item
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        ItemResponse? response = obj as ItemResponse;
        if( response is null )
            return false;

        return Id == response.Id
               && ListId == response.ListId
               && Name == response.Name
               && CreatedOn == response.CreatedOn
               && CheckedOn == response.CheckedOn
               && Count == response.Count;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Id.GetHashCode()
               ^ ListId.GetHashCode()
               ^ Name.GetHashCode()
               ^ CreatedOn.GetHashCode()
               ^ CheckedOn.GetHashCode()
               ^ Count.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}