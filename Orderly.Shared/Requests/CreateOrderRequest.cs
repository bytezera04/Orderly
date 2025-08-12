
using System.ComponentModel.DataAnnotations;

namespace Orderly.Shared.Requests
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Product public ID is required")]
        public string ProductPublicId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be above zero")]
        public int Quantity { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(250)]
        public string Notes { get; set; }
    }
}
