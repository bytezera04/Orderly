
using Orderly.Shared.Dtos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("ProductTbl")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string PublicId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public ProductCategory Category { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Stock { get; set; }

        public int Sales { get; set; } = 0;

        public string? OwnerId { get; set; }

        public AppUser? Owner { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public bool IsSeeded { get; set; }

        public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ProductDto ToDto()
        {
            return new ProductDto
            {
                PublicId = PublicId,
                Name = Name,
                Description = Description,
                Category = Category.ToString(),
                Price = Price,
                Stock = Stock,
                Sales = Sales,
                Owner = Owner?.ToDto(),
                CreatedAt = CreatedAt,

                Tags = Tags
                    .Select(t => t.Name)
                    .ToList(),

                IsDeleted = IsDeleted
            };
        }
    }

    public enum ProductCategory
    {
        Electronics = 1,
        Clothing = 2,
        Books = 3,
        Toys = 4,
        Home = 5
    }
}
