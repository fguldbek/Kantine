using Core.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KantineApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        
        private string serverUrl = "https://kantineserverapi.azurewebsites.net";

        public LoginService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // Henter den loggede bruger fra localStorage, og gemmer det i user
        public async Task<Employee?> GetUserLoggedIn()
        {
            var user = await _localStorage.GetItemAsync<Employee>("user");
            return user;
        }

        // Logger brugeren ind med brugernavn og adgangskode
        public async Task<bool> Login(string username, string password)
        {
                // Lav et GET-kald til API'et
                var response = await _httpClient.GetAsync($"{serverUrl}/api/login/login?username={username}&password={password}");

                // Check om statuskoden er succes
                if (response.IsSuccessStatusCode)
                {
                    // Forsøg at læse svaret som tekst
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Tjek om svaret indeholder en fejlmeddelelse
                    if (responseContent.Contains("Invalid username or password", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Login failed: Invalid username or password.");
                        return false;
                    }

                    // Hvis svaret ikke indeholder fejl, forsøg at parse brugeren
                    var user = await response.Content.ReadFromJsonAsync<Employee>();
                    if (user != null)
                    {
                        await _localStorage.SetItemAsync("user", user);
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine($"Unexpected error: {response.StatusCode}");
                }
                
            return false;
        }



        // Logger brugeren ud ved at fjerne brugerinfo fra localStorage
        public async Task Logout()
        {
            Console.WriteLine("Logout: Fjerner bruger fra localStorage.");
            await _localStorage.RemoveItemAsync("user");
        }
    }
}
