
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Services;
using Orderly.Shared.Dtos;
using Orderly.Shared.Helpers;

namespace Orderly.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _Context;

        private readonly UserManager<AppUser> _UserManager;

        private readonly OrderService _OrderService;

        private readonly ChatService _ChatService;

        public ChatController(AppDbContext context, UserManager<AppUser> userManager, OrderService orderService, ChatService chatService)
        {
            _Context = context;
            _UserManager = userManager;
            _OrderService = orderService;
            _ChatService = chatService;
        }

        [HttpGet("previews")]
        public async Task<IActionResult> GetChatPreviews()
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the chats

            List<ChatThread> chats = await _ChatService.GetUserChats(userId);

            // Get the chat previews

            List<ChatThreadPreviewDto> previews = new List<ChatThreadPreviewDto>();

            foreach (ChatThread chat in chats)
            {
                // Get the preview message

                ChatMessage? previewMessage = chat.Messages
                    .OrderByDescending(ct => ct.CreatedAt)
                    .FirstOrDefault();

                // Get the other participant

                ChatParticipant? otherParticipant = chat.Participants
                    .Where(cp => cp.UserId != userId)
                    .FirstOrDefault();

                // Get the last activity (preview message time or chat creation date)

                DateTime lastActivity = previewMessage is not null
                    ? previewMessage.CreatedAt
                    : chat.CreatedAt;

                // Attempt to determine the context public ID

                string? contextPublicId = null;

                if (chat.ContextType == ChatContext.Order)
                {
                    Order? order = await _OrderService.GetOrderFromIdAsync(chat.ContextId);

                    contextPublicId = order?.PublicId ?? null;
                }

                // Create the preview

                previews.Add(new ChatThreadPreviewDto
                {
                    ChatThread = chat.ToDto(),
                    PreviewMessage = previewMessage?.ToDto(),
                    OtherParticipant = otherParticipant?.User.ToDto(),
                    LastActivityAt = lastActivity,
                    ContextPublicId = contextPublicId,
                    IsDeleted = chat.IsDeleted
                });
            }

            // Respond with the result

            return Ok(previews);
        }

        [HttpGet("order-chat/{orderPublicId}")]
        public async Task<IActionResult> GetOrderChat(string orderPublicId)
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database order

            Order? order = await _OrderService.GetOrderFromPublicIdAsync(orderPublicId);

            if (order is null)
            {
                return NotFound();
            }

            // Get the database chat

            ChatThread? chat = await _ChatService.GetOrCreateOrderChatThreadAsync(order);

            if (chat is null)
            {
                return NotFound();
            }

            // The user must be a participant in this chat

            if (!chat.Participants.Any(p => p.UserId == userId))
            {
                return Forbid();
            }

            // Respond with the chat

            return Ok(chat.ToDto());
        }

        [HttpGet("{chatPublicId}/messages")]
        public async Task<IActionResult> GetChatMessages(
            string chatPublicId,
            [FromQuery] string? startMessagePublicId = null
        )
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database chat

            ChatThread? chat = await _ChatService.GetChatThreadFromPublicIdAsync(chatPublicId);

            if (chat is null)
            {
                return NotFound();
            }

            // The user must be a participant in this chat

            if (!chat.Participants.Any(p => p.UserId == userId))
            {
                return Forbid();
            }

            // Get the starting timestamp to use

            DateTime? startTimeStamp = null;

            if (!string.IsNullOrEmpty(startMessagePublicId))
            {
                ChatMessage? startMessage = await _ChatService.GetMessageByPublicIdAsync(startMessagePublicId);

                if (startMessage is not null)
                {
                    // Check this message is part of this chat and not another chat

                    if (startMessage.ChatThreadId != chat.Id)
                    {
                        return BadRequest();
                    }

                    // Get the timestamp

                    startTimeStamp = startMessage.CreatedAt;
                }
            }

            // Get the messages

            const int TAKE = 20;

            List<ChatMessage> messages = await _ChatService.GetChatMessagesPagedAsync(chat, startTimeStamp, TAKE);

            // Respond with the result

            List<ChatMessageDto> result = messages
                .OrderBy(cm => cm.CreatedAt)
                .Select(cm => cm.ToDto())
                .ToList();

            return Ok(result);
        }

        [HttpPost("{chatPublicId}")]
        public async Task<IActionResult> CreateChatMessage(string chatPublicId, [FromBody] ChatMessageDto message)
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database chat

            ChatThread? chat = await _ChatService.GetChatThreadFromPublicIdAsync(chatPublicId);

            if (chat is null)
            {
                return NotFound();
            }

            // The user must be a participant in this chat

            if (!chat.Participants.Any(p => p.UserId == userId))
            {
                return Forbid();
            }

            // Perform message validations

            string contentSanitized = message.Content.Trim();
            contentSanitized = contentSanitized.Replace("\r\n", "\n").Replace("\r", "\n");

            if (string.IsNullOrWhiteSpace(contentSanitized))
            {
                return BadRequest("Message cannot be empty");
            }

            if (contentSanitized.Length > 2000)
            {
                return BadRequest("Message is too long");
            }

            // Create the message

            await _ChatService.CreateChatMessage(new ChatMessage
            {
                PublicId = await PublicIdGeneration.GenerateChatMessageId(_Context),
                ChatThreadId = chat.Id,
                SenderId = userId,
                Content = contentSanitized
            });

            return Created();
        }
    }
}
