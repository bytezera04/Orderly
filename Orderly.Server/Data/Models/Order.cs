using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orderly.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("OrderTbl")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string PublicId { get; set; }

        public int? ProductId { get; set; }

        public Product? Product { get; set; }

        public string? CustomerId { get; set; }

        public AppUser? Customer { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public OrderDto ToDto()
        {
            return new OrderDto
            {
                PublicId = PublicId,
                Customer = Customer?.ToDto(),
                Product = Product?.ToDto(),
                Status = Status.ToString(),
                Quantity = Quantity,
                Price = Price,
                Notes = Notes,
                CreatedAt = CreatedAt,
                IsDeleted = IsDeleted
            };
        }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Complete,
        Cancelled
    }
}
