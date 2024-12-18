using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using Core.Models;
using ServerAPI.Repositories;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/login")] // Base URL for alle endpoints i denne controller
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _repo;
        
        public LoginController(ILoginRepository repo)
        {
            _repo = repo;
        }
        
        // Login endpoint, som håndterer login med brugernavn og password
        [HttpGet("login")]
        public async Task<object> Login(string username, string password)
        {
            // Henter brugeren fra repository baseret på de angivne brugernavn og password
            var user = await _repo.GetEmployeeByCredentialsAsync(username, password);

            // Hvis brugeren ikke findes, returneres en Unauthorized fejlbesked
            if (user == null)
                return new { Message = "Invalid username or password" }; // Returneres som et simpelt objekt

            // Hvis login er succesfuldt, returneres brugerens oplysninger (uden password for sikkerhed)
            return new 
            {
                user.Id, // Brugerens ID
                user.Name, // Brugerens navn
                user.Email, // Brugerens email
                user.Number, // Brugerens telefonnummer
                user.Skills, // Brugerens færdigheder
                user.Role // Brugerens rolle
            };
        }
    }
}