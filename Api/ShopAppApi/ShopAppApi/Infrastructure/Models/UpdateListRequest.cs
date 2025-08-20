namespace ShopAppApi.Infrastructure.Models;

/// <summary>
/// Request to delta update a list, list is changed to values provided in this request.
/// </summary>
public class UpdateListRequest
{
    /// <summary>
    /// Id of list to edit
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of list
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Items in list
    /// </summary>
    public List<UpdateItemRequest> Items { get; set; } = new();

    /// <summary>
    /// Compares current object with another one
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        UpdateListRequest? request = obj as UpdateListRequest;
        if( request is null )
            return false;

        return Id == request.Id
               && string.CompareOrdinal( Name, request.Name ) == 0
               && Items == request.Items;
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
               ^ Items.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// Validation
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return Id > 0 && !string.IsNullOrEmpty( Name ) && Items.Any() && Items.Select( i => i.IsValid() ).FirstOrDefault();
    }
}

/// <summary>
/// UpdateItemRequest
/// </summary>
public class UpdateItemRequest
{
    /// <summary>
    /// Id of item. Null for new item, set value to edit item.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Name of item
    /// </summary>
    public string Name { get; set; } = string.Empty;

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
        UpdateItemRequest? request = obj as UpdateItemRequest;
        if( request is null )
            return false;

        return ((!Id.HasValue && !request.Id.HasValue) || (Id.HasValue && Id == request?.Id))
               && string.CompareOrdinal( Name, request.Name ) == 0
               && Count == request.Count;
    }

    /// <summary>
    /// Calculates hash-code for current object
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return (Id?.GetHashCode() ?? 0)
               ^ Name.GetHashCode()
               ^ Count.GetHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// Validation
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty( Name ) && Count > 0;
    }
}