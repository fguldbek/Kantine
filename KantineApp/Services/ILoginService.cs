using Core.Models;

namespace KantineApp.Services;

public interface ILoginService
{
    Task<Employee?> GetUserLoggedIn();

    Task<bool> Login(string username, string password);
    
    Task Logout();

}