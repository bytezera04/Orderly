
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Orderly.Shared.Dtos
{
    public class UserDto
    {
        [ValidateNever]
        public string PublicId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}
