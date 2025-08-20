using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// UpdateListRequest
    /// </summary>
    public class UpdateListRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Items
        /// </summary>
        [JsonProperty( PropertyName = "items" )]
        public List<UpdateItemRequest> Items { get; set; } = new();
    }

    /// <summary>
    /// UpdateListRequest
    /// </summary>
    public class UpdateItemRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int? Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty( PropertyName = "count" )]
        public int Count { get; set; }
    }
}