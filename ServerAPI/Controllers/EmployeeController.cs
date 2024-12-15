using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using Core.Models;
using ServerAPI.Repositories;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository repo, ILogger<EmployeeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        [HttpPut]
        [Route("update")]
        public void UpdateEmployee(Employee employee){
            _repo.UpdateEmployee(employee);
        }
        
        
        
        
        // Add a new employee
        [HttpPost("add")]
        public IActionResult AddItem(Employee employee)
        {
            try
            {
                _repo.Add(employee);
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee.");
                return StatusCode(500, "Internal server error.");
            }
        }
        
        [HttpDelete]
        [Route("DeleteEmployeeById/{id:int}")]
        public IActionResult DeleteEmployeeById(int id) 
        {
                _repo.DeleteEmployeeById(id); // Call repository method to delete the item.
                return Ok(new { Message = $"Item with ID {id} deleted successfully." });
        }
        
        [HttpPut("ChangeRole/{id}")]
        public async Task<IActionResult> ChangeRole(int id, [FromBody] int newRole)
        {
            var result = await _repo.UpdateEmployeeRole(id, newRole);

            if (result)
            {
                return NoContent(); // Successfully updated, no content to return
            }

            return NotFound($"Employee with id {id} not found.");
        }

    
        
        
        [HttpGet]
        [Route("GetById/{UserId}")]
        public Employee? GetById(int UserId)
        {
            {
                return _repo.GetById(UserId);
            }

        }
        
        [HttpGet]
        [Route("GetAllEmployees")]
        public IEnumerable<Employee> GetAllEmployees()
        {
            {
                return _repo.GetAllEmployees();
            }

        }
        
    }
}
