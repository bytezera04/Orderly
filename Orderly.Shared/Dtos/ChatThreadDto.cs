
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Orderly.Shared.Dtos
{
    public class ChatThreadDto
    {
        [ValidateNever]
        public string PublicId { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string ContextType { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;
    }
}
