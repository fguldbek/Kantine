using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository mRepo;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventsRepository repo, ILogger<EventsController> logger)
        {
            mRepo = repo;
            _logger = logger;
        }
        
        [HttpPost("add")]
        public IActionResult AddEvent([FromBody] Events newEvent)
        {
            if (newEvent == null)
            {
                _logger.LogError("Received null event.");
                return BadRequest("Event cannot be null.");
            }

            try
            {
                // Kald til repositoryet for at tilføje eventet
                mRepo.AddEvent(newEvent);
                return Ok(new { message = "Event added successfully", eventId = newEvent.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add event.");
                return StatusCode(500, "Internal server error.");
            }
        }
        
        [HttpGet]
        [Route("GetAllEvents")]
        public IEnumerable<Events> GetAllEvents(){
            {
                return mRepo.GetAllEvents(); 
            }
           
        }


       
        }
    }
