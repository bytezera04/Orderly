
using Orderly.Shared.Dtos;
using Orderly.Shared.Responses;
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
            string sortBy,
            int page = 1
        )
        {
            // Build the URL

            string url = $"/api/marketplace?page={page}" +
                 $"&searchText={searchText}" +
                 $"&category={category}" +
                 $"&minPrice={minPrice}&maxPrice={maxPrice}" +
                 $"&inStockOnly={inStock}" +
                 $"&sortBy={sortBy}";

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
}
