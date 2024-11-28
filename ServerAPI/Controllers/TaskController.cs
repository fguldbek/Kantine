using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository mRepo;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository repo, ILogger<TaskController> logger){
            _logger = logger;
            mRepo = repo;
        }
 
       
        
        


    }
}