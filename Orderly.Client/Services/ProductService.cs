
using System.Text.Json;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Orderly.Shared.Dtos;
using Orderly.Client.Helpers;

namespace Orderly.Client.Services
{
    public class ProductService
    {
        private readonly HttpClient _HttpClient;

        public ProductService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<(bool Success, List<string>? ErrorMessages)> CreateProductAsync(ProductDto product)
        {
            try
            {
                HttpResponseMessage response = await _HttpClient.PostAsJsonAsync("/api/products", product);

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

        public async Task<(bool Success, List<string>? ErrorMessages)> UpdateProductAsync(ProductDto product)
        {
            try
            {
                HttpResponseMessage response = await _HttpClient.PutAsJsonAsync($"/api/products/{product.PublicId}", product);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();

                    List<string> errors = ValidationProblemsHelper.ParseValidationErrors(data);

                    if (errors.Any())
                    {
                        errors.Add("Failed to edit product.");
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

        public async Task<(bool Success, List<string>? ErrorMessages)> DeleteProductAsync(ProductDto product)
        {
            try
            {
                HttpResponseMessage response = await _HttpClient.DeleteAsync($"/api/products/{product.PublicId}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();

                    List<string> errors = ValidationProblemsHelper.ParseValidationErrors(data);

                    if (errors.Any())
                    {
                        errors.Add("Failed to edit product.");
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

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _HttpClient.GetFromJsonAsync<List<ProductDto>>("/api/products") ?? new();
        }
    }
}
