
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Orderly.Shared.Dtos
{
    public class OrderDto
    {
        [ValidateNever]
        public string PublicId { get; set; } = string.Empty;

        public UserDto? Customer { get; set; } = new UserDto();

        [Required(ErrorMessage = "Status is required")]
        public ProductDto? Product { get; set; } = new ProductDto();

        public string Status { get; set; } = default!;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; } = default!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; } = default!;

        [Required(AllowEmptyStrings = true, ErrorMessage = "Order notes are required")]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;

        public string CustomerDisplayName
        {
            get
            {
                return Customer?.FullName ?? "(Deleted User)";
            }
        }

        public string ProductDisplayName
        {
            get
            {
                return Product?.Name ?? "(Deleted Product)";
            }
        }

        public string ProductOwnerDisplayName
        {
            get
            {
                return Product?.OwnerDisplayName ?? "(Deleted Product)";
            }
        }

        public bool IsReceivedOrder(string? userPublicId)
        {
            if (string.IsNullOrEmpty(userPublicId))
            {
                return false;
            }

            return Product?.Owner?.PublicId == userPublicId;
        }

        public bool IsPlacedOrder(string? userPublicId)
        {
            if (string.IsNullOrEmpty(userPublicId))
            {
                return false;
            }

            return Customer?.PublicId == userPublicId;
        }
    }
}
