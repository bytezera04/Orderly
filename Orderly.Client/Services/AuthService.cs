
using Microsoft.AspNetCore.Components.Authorization;
using Orderly.Client.Providers;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using static Orderly.Client.Pages.Login;

namespace Orderly.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                // Notify auth state changed

                if (_authStateProvider is ServerAuthenticationStateProvider customProvider)
                {
                    customProvider.NotifyUserAuthentication();
                }

                return true;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
            return response.IsSuccessStatusCode;
        }
    }

    public class LoginModel
    {
        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, MinLength(6)]
        public string? Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string? ConfirmPassword { get; set; }
    }
}
