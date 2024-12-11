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
        public IEnumerable<Events> GetAllEvents()
        {
            {
                return mRepo.GetAllEvents();
            }

        }

        [HttpGet]
        [Route("GetEventById/{id}")]
        public ActionResult<Events> GetEventById(int id)
        {
            {
                var eventItem = mRepo.GetEventById(id).FirstOrDefault();
                if (eventItem == null)
                {
                    return NotFound($"Event with ID {id} not found.");
                }

                return Ok(eventItem);
            }
        }

        [HttpGet]
        [Route("GetEventTaskById/{eventId}/{taskId}")]
        public ActionResult<EventTask> GetEventTaskById(int eventId, int taskId)
        {
            var eventItem = mRepo.GetEventById(eventId).FirstOrDefault();
            if (eventItem == null)
            {
                return NotFound($"Event with ID {eventId} not found.");
            }

            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found in Event {eventId}.");
            }

            return Ok(task);

        }


        [HttpPut]
        [Route("AddTaskToEvent/{id}")]
        public async Task AddTask(int id, EventTask task)
        {
                mRepo.AddTask(id, task);
        }

        [HttpPut]
        [Route("AddAssignmentToTask/{eventId}/{taskId}")]
        public async Task<IActionResult> AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            // Find eventet og den relevante opgave
            var eventItem = mRepo.GetEventById(eventId).FirstOrDefault();
            if (eventItem == null)
            {
                return NotFound($"Event with ID {eventId} not found.");
            }

            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found.");
            }

            // Tilføj den nye assignment
            task.AssignmentList.Add(newAssignment);

            // Opdater EmployeeCapacity og EmployeesAssigned
            task.EmployeesAssigned = task.AssignmentList.Count;

            // Gem ændringerne i eventet
            await mRepo.UpdateItem(eventItem);

            return Ok(task);  // Returnér den opdaterede EventTask
        }
        
        [HttpGet]
        [Route("GetEmployeeAssignmentsById/{UserId:int}")]
        public IEnumerable<Events> GetEmployeeAssignmentsById(int UserId){
            try
            {
                return mRepo.GetEmployeeAssignmentsById(UserId); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all orders. Exception: {Message}", ex.Message);
                throw;
            }
        }
        
        [HttpGet]
        [Route("GetAssignmentsForTask/{eventId}/{taskId}")]
        public ActionResult<List<Assignment>> GetAssignmentsForTask(int eventId, int taskId)
        {
            var eventItem = mRepo.GetEventById(eventId).FirstOrDefault();
            if (eventItem == null)
            {
                return NotFound($"Event with ID {eventId} not found.");
            }

            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found.");
            }

            return Ok(task.AssignmentList);
        }
        
        [HttpPut]
        [Route("UpdateEvent")]
        public IActionResult UpdateItem([FromBody] Events product)
        {
            try
            {
                mRepo.UpdateItem(product);
                return Ok("Event updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("UpdateTask/{eventId}")]
        public async Task<IActionResult> UpdateTask(int eventId, [FromBody] EventTask updatedTask)
        {
            try
            {
                
                var existingEvent = mRepo.GetEventById(eventId).FirstOrDefault();
                if (existingEvent == null)
                {
                    return NotFound("Event not found");
                }

                
                var taskIndex = existingEvent.TaskList.FindIndex(t => t.Id == updatedTask.Id);
                if (taskIndex == -1)
                {
                    return NotFound("Task not found");
                }

               
                existingEvent.TaskList[taskIndex] = updatedTask;

                
                await mRepo.UpdateItem(existingEvent);

                return Ok("Task updated successfully");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        
    }
}
    
