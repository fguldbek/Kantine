namespace Kantine.Services
{
    public class UserSessionService
    {
        public bool IsLoggedIn { get; private set; }
        public event Action? OnChange;

        public void SetLoginStatus(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}