
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Services;
using Orderly.Shared.Dtos;
using Orderly.Shared.Helpers;

namespace Orderly.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _Context;

        private readonly UserManager<AppUser> _UserManager;

        private readonly ProductService _ProductService;

        public ProductController(AppDbContext context, UserManager<AppUser> userManager, ProductService productService)
        {
            _Context = context;
            _UserManager = userManager;
            _ProductService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProducts()
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the products

            List<Product> products = await _ProductService.GetProductsAsync(userId);

            List<ProductDto> productDtos = products
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.ToDto())
                .ToList();

            return Ok(productDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto newProductDto)
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Validate required fields exist

            if (string.IsNullOrWhiteSpace(newProductDto.Name)
                || newProductDto.Price <= 0)
            {
                return BadRequest();
            }

            // Enforce tag limit

            if (newProductDto.Tags.Count > 20)
            {
                return BadRequest();
            }

            // Parse the category

            ProductCategory category;

            if (Enum.TryParse<ProductCategory>(newProductDto.Category, ignoreCase: true, out var categoryEnum))
            {
                category = categoryEnum;
            }
            else
                return BadRequest();
            {
            }

            // Create the new product

            Product product = new Product
            {
                PublicId = await PublicIdGeneration.GenerateProductId(_Context),
                Name = newProductDto.Name,
                Description = newProductDto.Description,
                Category = category,
                Price = newProductDto.Price,
                Stock = newProductDto.Stock,
                OwnerId = userId
            };

            await _ProductService.AddProductAsync(product, newProductDto.Tags);

            return Created();
        }

        [HttpPut("{publicProductId}")]
        public async Task<IActionResult> UpdateProduct(string publicProductId, [FromBody] ProductDto updatedProductDto)
        {
            if (publicProductId != updatedProductDto.PublicId)
            {
                return BadRequest();
            }

            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database product

            Product? product = await _ProductService.GetProductFromPublicIdAsync(updatedProductDto.PublicId);

            if (product is null)
            {
                return NotFound();
            }

            // The user must be allowed to edit the product

            if (product.OwnerId != userId)
            {
                return Forbid();
            }

            // Enforce tag limit

            if (updatedProductDto.Tags.Count > 20)
            {
                return BadRequest();
            }

            // Parse the category

            ProductCategory category;

            if (!Enum.TryParse(updatedProductDto.Category, ignoreCase: true, out category))
            {
                return BadRequest();
            }

            // Update other fields

            product.Name = updatedProductDto.Name;
            product.Description = updatedProductDto.Description;
            product.Price = updatedProductDto.Price;
            product.Stock = updatedProductDto.Stock;
            product.IsDeleted = updatedProductDto.IsDeleted;

            product.Category = category;

            await _ProductService.UpdateProductAsync(product, updatedProductDto.Tags);

            return NoContent();
        }

        [HttpDelete("{publicProductId}")]
        public async Task<IActionResult> DeleteProduct(string publicProductId)
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database product

            Product? product = await _ProductService.GetProductFromPublicIdAsync(publicProductId);

            if (product is null)
            {
                return NotFound();
            }

            // The user must be allowed to delete the product

            if (product.OwnerId != userId)
            {
                return Forbid();
            }

            // Delete the product

            await _ProductService.DeleteProductAsync(publicProductId);

            return NoContent();
        }
    }
}
