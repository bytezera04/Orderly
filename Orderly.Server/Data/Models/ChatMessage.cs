
using Orderly.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string PublicId { get; set; }

        [Required]
        public int ChatThreadId { get; set; }

        public ChatThread ChatThread { get; set; } = default!;

        [Required]
        public string? SenderId { get; set; }

        public AppUser Sender { get; set; } = default!;

        [Required(ErrorMessage = "Message is required")]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public ChatMessageDto ToDto()
        {
            return new ChatMessageDto
            {
                PublicId = PublicId,
                Sender = Sender.ToDto(),
                Content = Content,
                CreatedAt = CreatedAt,
                IsDeleted = IsDeleted
            };
        }
    }
}
