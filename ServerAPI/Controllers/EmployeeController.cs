using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository mRepo;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository repo, ILogger<EmployeeController> logger)
        {
            _logger = logger;
            mRepo = repo;
        }

        [HttpPost]
        [Route("add")]
        public void AddItem(Employee product)
        {
            mRepo.Add(product);
        }


        // Modified GetUserId to accept an id
      
        [HttpGet]
        [Route("GetEmployee/{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            try
            {
                var employee = mRepo.GetById(id);

                if (employee != null)
                {
                    return Ok(employee); // Return the employee details
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