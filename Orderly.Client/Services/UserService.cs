
using Orderly.Shared.Dtos;
using System.Net.Http.Json;

namespace Orderly.Client.Services
{
    public class UserService
    {
        private readonly HttpClient _HttpClient;

        public UserService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<UserDto?> GetCurrentUser()
        {
            try
            {
                return await _HttpClient.GetFromJsonAsync<UserDto>("/api/user");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
