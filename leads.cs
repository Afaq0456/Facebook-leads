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
            // Get your Facebook access token
            string accessToken = "EAAYYVoZBWkpMBO9SKMwZBpxChQcLBDj1fiCkgp3vrHJVMqVGLJ7juUZArZByGLJClK83izhApjjX75Dzsydsx6g1Q14BZBOUHJtG9Lx5fb23NdMCbUS9BmhHMkCQoPOwT7ytXrtdjHGXxF6BJ3aI0WklE31OK2ZBk8bGx4RjFr98sr30bnVXc7nmyYCHitingkXKZAgKkwT";

            // Get the page ID
            string pageId = "1715609915527827";

            // Create an HTTP client
            HttpClient client = new HttpClient();

            // Set the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Make a request to the Facebook Graph API to get the campaigns for the page
            // Correct API endpoint for fetching campaigns for a Facebook Page
// Make a GET request to retrieve leads by ad group
HttpResponseMessage response = await client.GetAsync($"https://graph.facebook.com/v18.0/{pageId}/adaccounts?fields=campaigns{{id,name}}");


            // Check the response status code
            if (response.IsSuccessStatusCode)
            {
                // Get the response content
                string content = await response.Content.ReadAsStringAsync();

                // Deserialize the response content into a list of campaigns
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                List<Campaign> campaigns = JsonSerializer.Deserialize<List<Campaign>>(content, options);

                // Display the campaign data on the console
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
                // Handle the error
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
