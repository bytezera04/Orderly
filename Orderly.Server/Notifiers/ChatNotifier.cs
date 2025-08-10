
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Hubs;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Notifiers
{
    public class ChatNotifier : IChatNotifier
    {
        private readonly IHubContext<ChatHub> _HubContext;

        private readonly AppDbContext _Context;

        public ChatNotifier(IHubContext<ChatHub> hubContext, AppDbContext context)
        {
            _HubContext = hubContext;
            _Context = context;
        }

        public async Task NotifyMessageCreatedAsync(string chatPublicId, ChatMessageDto messageDto)
        {
            // Extract participant user IDs to send to

            ChatThread? chat = await _Context.ChatThreads
                .Include(ct => ct.Participants)
                .Where(ct => ct.PublicId == chatPublicId)
                .FirstOrDefaultAsync();

            if (chat is null)
            {
                return;
            }

            List<string> userIds = chat.Participants
                .Where(cp => cp.UserId != null)
                .Select(cp => cp.UserId!)
                .ToList();

            // Send the message to the participants

            var payload = new Tuple<string, ChatMessageDto>(chat.PublicId, messageDto);

            await _HubContext.Clients.Users(userIds)
                .SendAsync("MessageCreated", payload);
        }
    }

    public interface IChatNotifier
    {
        Task NotifyMessageCreatedAsync(string chatPublicId, ChatMessageDto messageDto);
    }
}
