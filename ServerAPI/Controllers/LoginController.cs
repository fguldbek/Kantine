using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using Core.Models;
using ServerAPI.Repositories;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _repo;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginRepository repo, ILogger<LoginController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        // Login endpoint
        [HttpGet("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
                var user = await _repo.GetEmployeeByCredentialsAsync(username, password);

                if (user == null)
                    return Unauthorized("Invalid username or password");

                // Include all necessary properties (excluding sensitive data like password)
                return Ok(new 
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Number,
                    user.Skills,
                    user.Role
                });
        }


    }
}
