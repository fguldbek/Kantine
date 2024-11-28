using Core;

namespace Kantine.Service;

public interface ICreateLoginService
{

    Task<User> GetUserLoggedIn();

    Task<bool> Login(string username, string password);
    Task Logout();
    
}
x