
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Controllers
{
    [ApiController]
    [Route("api/marketplace")]
    public class MarketplaceController : ControllerBase
    {
        private readonly AppDbContext _Context;

        public MarketplaceController(AppDbContext context)
        {
            _Context = context;
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
            const int PAGE_SIZE = 16;

            var query = _Context.Products
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.ToString() == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (inStockOnly is true)
            {
                query = query.Where(p => p.Stock > 0);
            }

            var totalCount = await query.CountAsync();

            List<ProductDto> products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Select(p => p.ToDto())
                .ToListAsync();



            return Ok(new {
                totalCount,
                products
            });
        }
    }
}
