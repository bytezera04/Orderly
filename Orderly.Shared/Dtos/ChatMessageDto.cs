
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Orderly.Shared.Dtos
{
    public class ChatMessageDto
    {
        [ValidateNever]
        public string PublicId { get; set; } = string.Empty;

        public UserDto? Sender { get; set; } = new UserDto();

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;
    }
}
