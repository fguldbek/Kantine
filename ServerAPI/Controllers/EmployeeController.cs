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

        public EmployeeController(IEmployeeRepository repo, ILogger<EmployeeController> logger){
            _logger = logger;
            mRepo = repo;
        }
        
        [HttpPost]
        [Route("add")]
        public void AddItem(Employee product){
            mRepo.Add(product);  
        }
        


    }
}
