using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// ItemResponse
    /// </summary>
    public class ItemResponse
    {
        /// <summary>
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty( PropertyName = "listId" )]
        public int? ListId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty( PropertyName = "createdOn" )]
        public System.DateTime? CreatedOn { get; set; }

        /// <summary>
        /// CheckedOn
        /// </summary>
        [JsonProperty( PropertyName = "checkedOn" )]
        public System.DateTime? CheckedOn { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty( PropertyName = "count" )]
        public int Count { get; set; }
    }
}