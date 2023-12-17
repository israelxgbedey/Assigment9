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

        // Add a user
        await AddUser(apiUrl);

        // Display the users
        await DisplayUsers(apiUrl);

        // Prompt the user for the ID to delete
        Console.WriteLine("Do you want to delete a user? (Y/N)");
        string deleteOption = Console.ReadLine().ToUpper();

        if (deleteOption == "Y")
        {
            Console.WriteLine("Enter the ID of the user to delete:");
            if (int.TryParse(Console.ReadLine(), out int userIdToDelete))
            {
                // Delete the user with the specified ID
                await DeleteUser(apiUrl, userIdToDelete);

                // Display the updated users list
                await DisplayUsers(apiUrl);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }
        else
        {
            Console.WriteLine("Delete operation skipped.");
        }
    }

    static async Task AddUser(string apiUrl)
    {
        Console.WriteLine("Please enter your name:");
        string userName = Console.ReadLine();

        Console.WriteLine("Please enter your email:");
        string userEmail = Console.ReadLine();

        var userData = new { Name = userName, Email = userEmail };

        using (HttpClient client = new HttpClient())
        {
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

    static async Task DisplayUsers(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Current Users:");
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

    static async Task DeleteUser(string apiUrl, int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{userId}");

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
