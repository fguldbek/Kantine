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
            return await _localStorage.GetItemAsync<Employee>("user");
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
                        _logger.LogInformation($"User logged in: ID = {user.Id}, Name = {user.Name}");
                        await _localStorage.SetItemAsync("user", user);
                        return true;
                    }
                }
                else
                {
                    _logger.LogWarning($"Login failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
            }

            return false;
        }

        public async Task Logout()
        {
            // Remove user information from localStorage to log out
            await _localStorage.RemoveItemAsync("user");
            _logger.LogInformation("User logged out.");
        }
    }
}
