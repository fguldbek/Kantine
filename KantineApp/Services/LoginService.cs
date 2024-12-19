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
            var loginModel = new { Username = username, Password = password };
            
                var response = await _httpClient.GetAsync($"{serverUrl}/api/login/login?username={username}&password={password}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<Employee>();
                    //hvis user ikke er null, så bliver det json element der bliver hentet fra response sat i localstorage.
                    if (user != null)
                    {
                        await _localStorage.SetItemAsync("user", user);
                        return true;
                    }
                }
                else
                {
                    //Console.WriteLine til, at få fejlkode, til debug.
                    Console.WriteLine($"Login fejlede med statuskode: {response.StatusCode}");
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
