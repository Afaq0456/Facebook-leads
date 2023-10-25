using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FetchFacebookLeads
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string accessToken = "Access Token";
            string pageId = "page ID";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

HttpResponseMessage response = await client.GetAsync($"https://graph.facebook.com/v18.0/{pageId}/adaccounts?fields=campaigns{{id,name}}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                List<Campaign> campaigns = JsonSerializer.Deserialize<List<Campaign>>(content, options);

                foreach (var campaign in campaigns)
                {
                    Console.WriteLine($"Campaign ID: {campaign.Id}");
                    Console.WriteLine($"Campaign Name: {campaign.Name}");

                    if (campaign.Fields != null)
                    {
                        foreach (var field in campaign.Fields)
                        {
                            Console.WriteLine($"Campaign Field: {field.Name}");
                            Console.WriteLine($"Campaign Field Value: {field.Value}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to fetch campaigns: {response.StatusCode}");
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
            }
        }
    }

    public class Campaign
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<Field>? Fields { get; set; } = null;
    }

    public class Field
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
