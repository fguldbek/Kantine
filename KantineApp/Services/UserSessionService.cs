using Blazored.LocalStorage;
using Core.Models;

namespace KantineApp.Services
{
    public class UserSessionService
    {
        private readonly ILocalStorageService _localStorage;

        public UserSessionService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public bool IsLoggedIn { get; private set; }
        public event Action? OnChange;

        // Henter loginstatus fra LocalStorage og opdaterer tilstanden
        public async Task LoadLoginStatusAsync()
        {
            var user = await _localStorage.GetItemAsync<Employee>("user");
            IsLoggedIn = user != null; // Tjekker om der findes en bruger i LocalStorage, er brugeren logget ind
            NotifyStateChanged(); // Giver besked om, at loginstatus er ændret
        }

        // Sætter loginstatus og giver besked om ændringen
        public void SetLoginStatus(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
            NotifyStateChanged();
        }

        // Afliver eventet, når tilstanden er ændret
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}