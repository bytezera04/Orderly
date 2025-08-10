
using Orderly.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("UserTbl")]
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(12)]
        public string PublicId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        [Required]
        public bool IsDeleted { get; set; } = false;

        public UserDto ToDto()
        {
            return new UserDto
            {
                PublicId = PublicId,
                FullName = FullName,
                Email = Email,
                IsDeleted = IsDeleted
            };
        }
    }
}
