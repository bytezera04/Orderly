
using System.Text.Json.Serialization;
using Orderly.Shared.Dtos;

namespace Orderly.Shared.Responses
{
    public class MarketplaceResponse
    {
        [JsonPropertyName("products")]
        public List<ProductDto> Products { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }
}
