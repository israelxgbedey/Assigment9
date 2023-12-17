using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulWebApi
{
    public class HttpClientHelper
    {
        private readonly HttpClient client;

        public HttpClientHelper(string baseUrl)
        {
            client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task AddUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/user", content);

            HandleResponse(response);
        }

        public async Task RemoveUser(int userId)
        {
            var response = await client.DeleteAsync($"api/user/{userId}");

            HandleResponse(response);
        }

        public async Task GetUser(int userId)
        {
            var response = await client.GetAsync($"api/user/{userId}");

            HandleResponse(response);
        }

        private void HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
}
