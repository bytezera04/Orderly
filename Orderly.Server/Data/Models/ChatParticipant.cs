using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orderly.Server.Data.Models
{
    [Table("ChatParticipantTbl")]
    public class ChatParticipant
    {
        [Required]
        public int ChatThreadId { get; set; }

        [Required]
        public ChatThread ChatThread { get; set; } = default!;

        [Required]
        public string? UserId { get; set; }

        public AppUser User { get; set; } = default!;
    }
}
