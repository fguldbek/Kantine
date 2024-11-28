using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository mRepo;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventsRepository repo, ILogger<EventsController> logger){
            _logger = logger;
            mRepo = repo;
        }
 
       
        
        


    }
}