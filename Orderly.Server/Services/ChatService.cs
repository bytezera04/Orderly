
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Hubs;
using Orderly.Server.Notifiers;
using Orderly.Shared.Dtos;
using Orderly.Shared.Helpers;

namespace Orderly.Server.Services
{
    public class ChatService
    {
        private readonly AppDbContext _Context;

        private readonly IHubContext<ChatHub> _HubContext;

        private readonly ChatNotifier _ChatNotifier;

        public ChatService(AppDbContext context, IHubContext<ChatHub> hubContext, ChatNotifier chatNotifier)
        {
            _Context = context;
            _HubContext = hubContext;
            _ChatNotifier = chatNotifier;
        }

        public async Task CreateChatMessage(ChatMessage message)
        {
            // Add this message

            await _Context.ChatMessages.AddAsync(message);

            await _Context.SaveChangesAsync();

            // Notify message event

            string chatPublicID = message.ChatThread.PublicId;

            await _ChatNotifier.NotifyMessageCreatedAsync(chatPublicID, message.ToDto());
        }

        public async Task<ChatMessage?> GetMessageByPublicIdAsync(string publicId)
        {
            return await _Context.ChatMessages
                .Where(cm => cm.PublicId == publicId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatMessage>> GetAllChatMessages(ChatThread chat)
        {
            return await _Context.ChatMessages
                .Include(cm => cm.Sender)
                .Where(cm => cm.ChatThreadId == chat.Id)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetChatMessagesPagedAsync(ChatThread chat, DateTime? startTimeStamp, int take)
        {
            var query = _Context.ChatMessages
                .Where(cm => cm.ChatThreadId == chat.Id);

            // If specified, apply the start timestamp for loading older messages

            if (startTimeStamp.HasValue)
            {
                query = query
                    .Where(cm => cm.CreatedAt < startTimeStamp.Value);
            }

            query = query
                .OrderByDescending(cm => cm.CreatedAt);

            List<ChatMessage> pagedMessages = await query
                .Take(take)
                .ToListAsync();

            return pagedMessages;
        }

        public async Task<ChatThread?> GetChatThreadFromContextIdAsync(int contextId)
        {
            return await _Context.ChatThreads
                .Include(ct => ct.Participants)
                    .ThenInclude(cp => cp.User)
                .Include(ct => ct.Messages)
                    .ThenInclude(cm => cm.Sender)
                .Where(ct => ct.ContextId == contextId)
                .FirstOrDefaultAsync();
        }

        public async Task<ChatThread?> GetChatThreadFromPublicIdAsync(string chatPublicId)
        {
            return await _Context.ChatThreads
                .Include(ct => ct.Participants)
                    .ThenInclude(cp => cp.User)
                .Include(ct => ct.Messages)
                    .ThenInclude(cm => cm.Sender)
                .Where(ct => ct.PublicId == chatPublicId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatThread>> GetUserChats(string userId)
        {
            return await _Context.ChatThreads
                 .Include(ct => ct.Participants)
                    .ThenInclude(cp => cp.User)
                .Include(ct => ct.Messages)
                    .ThenInclude(cm => cm.Sender)
                .Where(ct => ct.Participants.Any(cp => cp.UserId == userId))
                .ToListAsync();
        }

        public async Task<ChatThread?> GetOrCreateOrderChatThreadAsync(Order order)
        {
            // Get the chat

            ChatThread? chat = await GetChatThreadFromContextIdAsync(order.Id);

            // Create the chat if it doesn't exist

            if (chat is null)
            {
                // Create the chat

                await CreateOrderChatThreadAsync(order);

                // Reload the chat

                chat = await GetChatThreadFromContextIdAsync(order.Id);
            }

            return chat;
        }

        public async Task CreateOrderChatThreadAsync(Order order)
        {
            // Checks

            if (order.Product is null)
            {
                throw new Exception("Cannot create chat from order with null Product");
            }

            // Create this chat

            ChatThread chat = new ChatThread
            {
                PublicId = await PublicIdGeneration.GenerateChatThreadId(_Context),
                Subject = $"Order #{order.PublicId}",
                ContextType = ChatContext.Order,
                ContextId = order.Id
            };

            await _Context.ChatThreads.AddAsync(chat);

            await _Context.SaveChangesAsync(); // required so chat.Id is set

            // Add product as participant

            await _Context.ChatParticipants.AddAsync(new ChatParticipant
            {
                ChatThreadId = chat.Id,
                UserId = order.Product.OwnerId
            });

            // Add customer as participant

            await _Context.ChatParticipants.AddAsync(new ChatParticipant
            {
                ChatThreadId = chat.Id,
                UserId = order.CustomerId
            });

            await _Context.SaveChangesAsync();
        }
    }
}
