using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// Creation request of Item
    /// </summary>
    public class ItemCreationRequest
    {
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}
