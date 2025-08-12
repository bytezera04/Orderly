
using Orderly.Shared.Dtos;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Orderly.Client.Services
{
    public class MarketplaceService
    {
        private readonly HttpClient _HttpClient;

        public MarketplaceService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<MarketplaceResponse?> GetProductsAsync(
            string? searchText,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStock,
            int page = 1
        )
        {
            // Build the URL

            string url = $"/api/marketplace?page={page}" +
                 $"&searchText={searchText}" +
                 $"&minPrice={minPrice}&maxPrice={maxPrice}" +
                 $"&inStockOnly={inStock}";

            // Send the request

            try
            {
                return await _HttpClient.GetFromJsonAsync<MarketplaceResponse>(url);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class MarketplaceResponse
    {
        [JsonPropertyName("products")]
        public List<ProductDto> Products { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }
}
