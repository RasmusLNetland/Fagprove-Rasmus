using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// Creation request for shopping lists
    /// </summary>
    public class ListCreationRequest
    {
        /// <summary>
        /// Gets or sets name of shopping list
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets list of shopping items
        /// </summary>
        [JsonProperty( PropertyName = "listItems" )]
        public IList<string> ListItems { get; set; }
    }
}