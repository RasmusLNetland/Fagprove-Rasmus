using Newtonsoft.Json;

namespace ShopAppFrontend.Infrastructure.DataStorage.ShopAppApi.Models
{
    /// <summary>
    /// List response
    /// </summary>
    public class ListResponse
    {
        /// <summary>
        /// Gets or sets id of list
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets name of list
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets when list was created
        /// </summary>
        [JsonProperty( PropertyName = "createdOn" )]
        public System.DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets when list was completed
        /// </summary>
        [JsonProperty( PropertyName = "completedOn" )]
        public System.DateTime? CompletedOn { get; set; }

        /// <summary>
        /// Gets or sets id of user who created the list
        /// </summary>
        [JsonProperty( PropertyName = "userId" )]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets shopping items in the list
        /// </summary>
        [JsonProperty( PropertyName = "items" )]
        public IList<ItemResponse> Items { get; set; }
    }
}