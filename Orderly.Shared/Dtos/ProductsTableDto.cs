
namespace Orderly.Shared.Dtos
{
    public class ProductsTableDto
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public string SortColumn { get; set; } = string.Empty;

        public string SortDirection { get; set; } = string.Empty;

        public string PagePath { get; set; } = string.Empty;

        public bool CanEdit { get; set; } = false;
    }
}
