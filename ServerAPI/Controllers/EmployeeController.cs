using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/employee")] // Base-rute for alle endpoints i denne controller
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repo; // Interface til employee repository
        
        public EmployeeController(IEmployeeRepository repo)
        {
            _repo = repo; // Initialiserer repository
        }
        
        // Endpoint til at opdatere en medarbejder
        [HttpPut]
        [Route("update")]
        public void UpdateEmployee(Employee employee)
        {
            // Kalder repository-metoden for at opdatere medarbejderen
            _repo.UpdateEmployee(employee);
        }

        // Endpoint til at tilføje en ny medarbejder
        [HttpPost("add")]
        public void AddItem(Employee employee)
        {
            // Tilføjer medarbejderen via repository
            _repo.Add(employee);
        }

        // Endpoint til at slette en medarbejder baseret på deres ID
        [HttpDelete]
        [Route("DeleteEmployeeById/{id:int}")]
        public async Task DeleteEmployeeById(int id)
        {
                await _repo.DeleteEmployeeById(id);  // Asynkron metode i repository
        }


        // Endpoint til at ændre en medarbejders rolle baseret på ID.
        // [FromBody] bruges her til at hente værdien af newRole fra request body,
        // i stedet for at sende den som en parameter i URL'en
        [HttpPut("ChangeRole/{id}")]
        public async Task<bool> ChangeRole(int id, [FromBody] int newRole)
        {
            // Opdaterer medarbejderens rolle via repository
            return await _repo.UpdateEmployeeRole(id, newRole);
        }

        // Endpoint til at hente en medarbejder baseret på deres UserId
        [HttpGet]
        [Route("GetById/{UserId}")]
        public Employee? GetById(int UserId)
        {
            // Returnerer medarbejderen fra repository
            return _repo.GetById(UserId);
        }

        // Endpoint til at hente alle medarbejdere
        [HttpGet]
        [Route("GetAllEmployees")]
        public IEnumerable<Employee> GetAllEmployees()
        {
            // Returnerer alle medarbejdere via repository
            return _repo.GetAllEmployees();
        }
    }
}
