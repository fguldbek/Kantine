using Core.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Kantine.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<LoginService> _logger;
        
        private string serverUrl = "http://localhost:5002";

        public LoginService(ILocalStorageService localStorage, HttpClient httpClient, ILogger<LoginService> logger)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Employee?> GetUserLoggedIn()
        {
            // Retrieve the user from localStorage
            var user = await _localStorage.GetItemAsync<Employee>("user");
            Console.WriteLine($"GetUserLoggedIn: Retrieved user: {user?.Name ?? "null"}");
            return user;
        }

        public async Task<bool> Login(string username, string password)
        {
            var loginModel = new { Username = username, Password = password };

            try
            {
                var response = await _httpClient.GetAsync($"{serverUrl}/api/login/login?username={username}&password={password}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<Employee>();
                    if (user != null)
                    {
                        Console.WriteLine($"Login: Storing user in localStorage: {user.Name}");
                        _logger.LogInformation($"User logged in: ID = {user.Id}, Name = {user.Name}");
                        await _localStorage.SetItemAsync("user", user);
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine($"Login failed with status code: {response.StatusCode}");
                    _logger.LogWarning($"Login failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                _logger.LogError(ex, "Error occurred during login.");
            }

            return false;
        }

        public async Task Logout()
        {
            // Remove user information from localStorage to log out
            Console.WriteLine("Logout: Removing user from localStorage.");
            await _localStorage.RemoveItemAsync("user");
            _logger.LogInformation("User logged out.");
        }
    }
}
