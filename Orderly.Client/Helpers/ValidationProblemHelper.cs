
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Orderly.Client.Helpers
{
    public static class ValidationProblemsHelper
    {
        public static List<string> ParseValidationErrors(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<string>
                {
                    "An unknown error occurred."
                };
            }

            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("errors", out JsonElement errorsElement))
                {
                    var messages = new List<string>();

                    foreach (var property in errorsElement.EnumerateObject())
                    {
                        string field = property.Name;
                        foreach (var message in property.Value.EnumerateArray())
                        {
                            messages.Add($"{field}: {message.GetString()}");
                        }
                    }

                    return messages;
                }
                else if (doc.RootElement.TryGetProperty("title", out JsonElement title))
                {
                    return new List<string>
                    {
                        title.GetString() ?? "An unknown error occurred."
                    };
                }
            }
            catch
            {
                return new List<string>
                {
                    "Failed to parse error response."
                };
            }

            return new List<string>
            {
                "An unknown error occurred."
            };
        }
    }
}
