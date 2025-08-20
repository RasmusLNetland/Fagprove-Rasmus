namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Response of List
/// </summary>
public class ListResponse
{
    /// <summary>
    /// Id of list
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of list
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// When list was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// When list was completed
    /// </summary>
    public DateTime? CompletedOn { get; set; }

    /// <summary>
    /// Id of user who created the list
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Shopping items in the list
    /// </summary>
    public IEnumerable<ItemResponse> Items { get; set; }

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        ListResponse? response = obj as ListResponse;
        if( response is null )
            return false;

        return Id == response.Id
               && Name == response.Name
               && CreatedOn == response.CreatedOn
               && CompletedOn == response.CompletedOn
               && UserId == response.UserId
               && Items == response.Items;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Id.GetHashCode()
               ^ Name.GetHashCode()
               ^ CreatedOn.GetHashCode()
               ^ CompletedOn.GetHashCode()
               ^ UserId.GetHashCode()
               ^ Items.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}