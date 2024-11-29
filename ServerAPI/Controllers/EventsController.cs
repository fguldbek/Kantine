using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository mRepo;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventRepository repo, ILogger<EventsController> logger){
            _logger = logger;
            mRepo = repo;
        }
        
        [HttpPost("add")]
        public IActionResult AddEvent([FromBody] Events newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event cannot be null.");
            }

            try
            {
                mRepo.AddEvent(newEvent);
                return Ok(new { message = "Event added successfully", eventId = newEvent.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add event.");
                return StatusCode(500, "Internal server error.");
            }
        }
        }
    
    }
