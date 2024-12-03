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

        // Get employee by ID
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var employee = _repo.GetById(id);

                if (employee != null)
                {
                    return Ok(employee);
                }

                return NotFound($"No employee found with ID {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee by ID: {Id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
