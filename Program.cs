using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        string apiUrl = "http://ec2-18-223-122-221.us-east-2.compute.amazonaws.com:5143/api/user";

        Console.WriteLine("Please enter your name:");
        string userName = Console.ReadLine();

        Console.WriteLine("Please enter your email:");
        string userEmail = Console.ReadLine();

        var userData = new { Name = userName, Email = userEmail };

        using (HttpClient client = new HttpClient())
        {
            // Convert data to JSON and then to content
            var json = JsonConvert.SerializeObject(userData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
