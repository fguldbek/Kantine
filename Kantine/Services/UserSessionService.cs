using System;

namespace Kantine.Services
{
    public class UserSessionService
    {
        public bool IsLoggedIn { get; private set; }
        public event Action OnChange; // Event for at informere andre komponenter om ændringer

        public void SetLoginStatus(bool status)
        {
            IsLoggedIn = status;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}