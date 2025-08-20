namespace ShopAppApi.Infrastructure.Models
{
    /// <summary>
    /// Creation Request for shopping items
    /// </summary>
    public class ItemCreationRequest
    {
        /// <summary>
        /// Name of shopping item
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Count of item
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            ItemCreationRequest? request = obj as ItemCreationRequest;
            if( request is null )
                return false;

            return Name == request.Name
                   && Count == request.Count;
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Name.GetHashCode()
                   ^ Count.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}