/*using Core;
using Blazored.LocalStorage;

namespace Kantine.Service;

public class CreateLoginService : ICreateLoginService
{
    private ILocalStorageService localStorage { get; set; }

    public LoginServiceClientSide(ILocalStorageService ls)
    {
        localStorage = ls;
    }

    public async Task<User?> GetUserLoggedIn()
    {
        var res = await localStorage.GetItemAsync<User>("user");
        return res;
    }

    public async Task<bool> Login(string username, string password)
    {
        if (username.Equals("peter") && password.Equals("1234"))
        {
            User user = new User { Username = username, Password = "verfied", Role = "admin", UserId = 1, BuyerId = 1 };
            await localStorage.SetItemAsync("user", user);
            return true;
        }
        if (username.Equals("kent") && password.Equals("1234"))
        {
            User user = new User { Username = username, Password = "verfied", Role = "admin", UserId = 2, BuyerId = 2 };
            await localStorage.SetItemAsync("user", user);
            return true;
        }
        if (username.Equals("johan") && password.Equals("1234"))
        {
            User user = new User { Username = username, Password = "verfied", Role = "admin", UserId = 3, BuyerId = 3 };
            await localStorage.SetItemAsync("user", user);
            return true;
        }
        return false;
    }

    // Implementer Logout-metoden
    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("user"); // Fjerner "user" fra localStorage ved logout
    }
}
*/