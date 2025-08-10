using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Hubs;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Services
{
    public class ProductService
    {
        private readonly AppDbContext _Context;

        private readonly IHubContext<ProductHub> _HubContext;

        public ProductService(AppDbContext context, IHubContext<ProductHub> hubContext)
        {
            _Context = context;
            _HubContext = hubContext;
        }

        public async Task AddProductAsync(Product product, List<string> tags)
        {
            // Add this product

            _Context.Products.Add(product);

            await _Context.SaveChangesAsync(); // Required so product.Id is set

            // Set the product tags

            await _Context.ProductTags.AddRangeAsync(
                tags.Select(t => new ProductTag
                {
                    ProductId = product.Id,
                    Name = t
                })
            );

            await _Context.SaveChangesAsync();

            // Broadcast event to product owner

            if (product.OwnerId is not null)
            {
                await _HubContext.Clients.User(product.OwnerId)
                    .SendAsync("ProductCreated", product.ToDto());
            }
        }

        public async Task UpdateProductAsync(Product updatedProduct, List<string> tags)
        {
            // Try to find this product

            Product? product = await _Context.Products
                .Where(p => p.Id == updatedProduct.Id)
                .Include(p => p.Owner)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync();

            if (product is null)
            {
                throw new Exception("Attempted to update a product that does not exist");
            }

            // Update the fields that support updating and save changes

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.IsDeleted = updatedProduct.IsDeleted;
            product.Tags = updatedProduct.Tags;

            await _Context.SaveChangesAsync();

            // Clear and set the tags

            _Context.ProductTags.RemoveRange(
                _Context.ProductTags.Where(pt => pt.ProductId == updatedProduct.Id)
            );

            await _Context.ProductTags.AddRangeAsync(
                tags.Select(t => new ProductTag
                {
                    ProductId = product.Id,
                    Name = t
                })
            );

            await _Context.SaveChangesAsync();

            // Broadcast even to product owner

            if (product.OwnerId is not null)
            {
                await _HubContext.Clients.User(product.OwnerId)
                    .SendAsync("ProductUpdated", product.ToDto());
            }
        }

        public async Task DeleteProductAsync(string productPublicId)
        {
            // Try to find this product

            Product? product = await _Context.Products
                    .Where(p => p.PublicId == productPublicId)
                    .FirstOrDefaultAsync();

            if (product is null)
            {
                return;
            }

            // Delete the product

            _Context.Products.Remove(product);

            await _Context.SaveChangesAsync();

            // Broadcast event to product owner

            if (product.OwnerId is not null)
            {
                await _HubContext.Clients.User(product.OwnerId)
                    .SendAsync("ProductDeleted", product.PublicId);
            }
        }

        public async Task<List<Product>> GetProductsAsync(string userId)
        {
            // Get all the products owned by the user

            return await _Context.Products
                .Include(p => p.Owner)
                .Include(p => p.Tags)
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<Product?> GetProductFromPublicIdAsync(string publicId)
        {
            return await _Context.Products
                .Include(p => p.Owner)
                .Include(p => p.Tags)
                .Where(p => p.PublicId == publicId)
                .FirstOrDefaultAsync();
        }
    }
}
