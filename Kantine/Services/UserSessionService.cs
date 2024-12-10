using Blazored.LocalStorage;
using Core.Models;


namespace Kantine.Services
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

        public async Task LoadLoginStatusAsync()
        {
            var user = await _localStorage.GetItemAsync<Employee>("user");
            IsLoggedIn = user != null; //tjekker om der findes en bruger i LocalStorage, er brugeren logget ind 
            NotifyStateChanged(); // Giver besked om, at looginstatus er ændret 
        }

        public void SetLoginStatus(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}