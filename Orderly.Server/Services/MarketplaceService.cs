
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using System.Threading.Tasks;

namespace Orderly.Server.Services
{
    public class MarketplaceService
    {
        private readonly AppDbContext _Context;

        private readonly IConfiguration _Config;

        public MarketplaceService(AppDbContext context, IConfiguration config)
        {
            _Context = context;
            _Config = config;
        }

        public async Task<(int TotalCount, List<Product> Products)> GetProducts(
            string? searchText,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStockOnly,
            int page,
            int pageSize
        )
        {
            // Query products

            var query = _Context.Products
                .Where(p => !p.IsDeleted);

            // Apply search filter

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchText) ||
                    p.Description.Contains(searchText)
                );
            }

            // Apply category filter

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.ToString() == category);
            }

            // Apply min and max price range

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Apply in stock only

            if (inStockOnly is true)
            {
                query = query.Where(p => p.Stock > 0);
            }

            // In demo mode, only seeded products (not user-generated products) can appear
            // in the public marketplace

            if (_Config.GetValue<bool>("DemoMode") is true)
            {
                query = query.Where(p => p.IsSeeded);
            }

            // Capture the total count

            int totalCount = await query.CountAsync();

            // Get the products for the specified page

            List<Product> products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new(totalCount, products);
        }
    }
}
