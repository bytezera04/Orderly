
using Orderly.Client.Helpers;
using Orderly.Shared.Dtos;
using Orderly.Shared.Requests;
using System.Net.Http;
using System.Net.Http.Json;

namespace Orderly.Client.Services
{
    public class OrderService
    {
        private readonly HttpClient _HttpClient;

        public OrderService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<(bool Success, List<string>? ErrorMessages)> CreateOrderAsync(CreateOrderRequest req)
        {
            try
            {
                HttpResponseMessage response = await _HttpClient.PostAsJsonAsync("/api/orders", req);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();

                    List<string> errors = ValidationProblemsHelper.ParseValidationErrors(data);

                    if (!errors.Any())
                    {
                        errors.Add("Failed to create order.");
                    }

                    return (false, errors);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<OrderDto?> GetOrder(string orderPublicId)
        {
            try
            {
                return await _HttpClient.GetFromJsonAsync<OrderDto>($"/api/orders/{orderPublicId}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool Success, List<string>? ErrorMessages)> UpdateOrderAsync(OrderDto order)
        {
            try
            {
                HttpResponseMessage response = await _HttpClient.PutAsJsonAsync($"/api/orders/{order.PublicId}", order);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();

                    List<string> errors = ValidationProblemsHelper.ParseValidationErrors(data);

                    if (!errors.Any())
                    {
                        errors.Add("Failed to create product.");
                    }

                    return (false, errors);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<List<OrderDto>> GetPlacedOrdersAsync()
        {
            return await _HttpClient.GetFromJsonAsync<List<OrderDto>>("api/orders/placed") ?? new();
        }

        public async Task<List<OrderDto>> GetReceivedOrdersAsync()
        {
            return await _HttpClient.GetFromJsonAsync<List<OrderDto>>("api/orders/received") ?? new();
        }
    }
}
