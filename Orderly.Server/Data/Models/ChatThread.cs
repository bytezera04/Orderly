using Orderly.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("ChatThreadTbl")]
    public class ChatThread
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string PublicId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

        [Required]
        public ChatContext ContextType { get; set; }

        [Required]
        public int ContextId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        public ChatThreadDto ToDto()
        {
            return new ChatThreadDto
            {
                PublicId = PublicId,
                Subject = Subject,
                ContextType = ContextType.ToString(),
                CreatedAt = CreatedAt,
                IsDeleted = IsDeleted
            };
        }
    }

    public enum ChatContext
    {
        Order
    }
}
