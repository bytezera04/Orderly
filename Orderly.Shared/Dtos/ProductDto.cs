
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Orderly.Shared.Dtos
{
    public class ProductDto
    {
        [ValidateNever]
        public string PublicId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = default!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; } = default!;

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; } = default!;

        public int Sales { get; set; } = 0;

        public UserDto? Owner { get; set; } = new UserDto();

        public DateTime CreatedAt { get; set; } = default!;

        [Length(0, 20, ErrorMessage = "You can only have upto 20 tags")]
        public List<string> Tags { get; set; } = new List<string>();

        public bool IsDeleted { get; set; } = false;

        public string OwnerDisplayName
        {
            get
            {
                return Owner?.FullName ?? "(Deleted User)";
            }
        }
    }
}
