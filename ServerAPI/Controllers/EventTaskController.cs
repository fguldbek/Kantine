using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/eventTask")]
    public class EventTaskController : ControllerBase
    {
        private readonly ITaskRepository mRepo;
        private readonly ILogger<EventTaskController> _logger;

        public EventTaskController(ITaskRepository repo, ILogger<EventTaskController> logger){
            _logger = logger;
            mRepo = repo;
        }
    }
}