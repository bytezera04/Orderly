
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Services;
using Orderly.Shared.Dtos;
using Orderly.Shared.Responses;

namespace Orderly.Server.Controllers
{
    [ApiController]
    [Route("api/marketplace")]
    public class MarketplaceController : ControllerBase
    {
        private readonly MarketplaceService _MarketplaceService;

        public MarketplaceController(MarketplaceService marketplaceService)
        {
            _MarketplaceService = marketplaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMarketplaceProducts(
            string? searchText,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStockOnly,
            int page = 1
        )
        {
            // Load the products for this page

            const int PAGE_SIZE = 16;

            var (totalCount, products) = await _MarketplaceService.GetProducts(
                searchText,
                category,
                minPrice,
                maxPrice,
                inStockOnly,
                page,
                PAGE_SIZE
            );

            // Respond with results

            List<ProductDto> productDtos = products
                .Select(p => p.ToDto())
                .ToList();

            return Ok(new MarketplaceResponse
            {
                TotalCount = totalCount,
                Products = productDtos
            });
        }
    }
}
