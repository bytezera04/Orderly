
namespace Orderly.Shared.Dtos
{
    public class ChatThreadPreviewDto
    {
        public ChatThreadDto? ChatThread { get; set; } = new ChatThreadDto();

        public ChatMessageDto? PreviewMessage { get; set; } = new ChatMessageDto();

        public UserDto? OtherParticipant { get; set; } = new UserDto();

        public DateTime LastActivityAt { get; set; } = default!;

        public string? ContextPublicId { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;

        public string OtherParticipantDisplayName
        {
            get
            {
                return OtherParticipant?.FullName ?? "(Deleted User)";
            }
        }
    }
}
