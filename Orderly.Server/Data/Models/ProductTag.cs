
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("ProductTagTbl")]
    public class ProductTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
