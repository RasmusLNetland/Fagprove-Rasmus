using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// List details response
    /// </summary>
    public class ListDetailsResponse
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        [JsonProperty( PropertyName = "items" )]
        public IEnumerable<ItemResponse> Items { get; set; }
    }
}