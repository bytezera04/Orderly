
using Orderly.Shared.Dtos;
using System.Net.Http.Json;

namespace Orderly.Client.Services
{
    public class ChatService
    {
        private readonly HttpClient _HttpClient;

        public ChatService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<List<ChatThreadPreviewDto>?> GetChatPreviews()
        {
            try
            {
                return await _HttpClient.GetFromJsonAsync<List<ChatThreadPreviewDto>>("/api/chat/previews");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ChatThreadDto?> GetChatAsync(string contextPublicId)
        {
            try
            {
                return await _HttpClient.GetFromJsonAsync<ChatThreadDto>($"/api/chat/order-chat/{contextPublicId}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets messages from a chat
        /// </summary>
        /// <param name="chat">
        ///     The chat
        /// </param>
        /// <param name="startMessagePublicId">
        ///     The start message public ID, this indicates the message to start from, used for
        ///     offsetting or paging
        /// </param>
        /// <returns>
        ///     The messages found
        /// </returns>
        public async Task<List<ChatMessageDto>?> GetChatMessagesAsync(ChatThreadDto chat, string? startMessagePublicId = null)
        {
            try
            {
                string params_ = string.Empty;

                if (!string.IsNullOrEmpty(startMessagePublicId))
                {
                    params_ += $"?startMessagePublicId={startMessagePublicId}";
                }

                return await _HttpClient.GetFromJsonAsync<List<ChatMessageDto>>($"/api/chat/{chat.PublicId}/messages{params_}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task CreateChatMessageAsync(ChatThreadDto chat, ChatMessageDto message)
        {
            await _HttpClient.PostAsJsonAsync($"/api/chat/{chat.PublicId}", message);
        }
    }
}
