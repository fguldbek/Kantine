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
        public async Task AddEvent(Events newEvent)
        {
            if (newEvent == null)
            {
                _logger.LogError("Received null event.");
                return;
            }

            mRepo.AddEvent(newEvent);
        }

        [HttpGet("GetAllEvents")]
        public IEnumerable<Events> GetAllEvents()
        {
            return mRepo.GetAllEvents();
        }

        [HttpGet("GetEventById/{id}")]
        public Events GetEventById(int id)
        {
            var eventItem = mRepo.GetEventById(id).FirstOrDefault();
            return eventItem ?? new Events();
        }

        [HttpGet("GetEventTaskById/{eventId}/{taskId}")]
        public EventTask GetEventTaskById(int eventId, int taskId)
        {
            var eventItem = mRepo.GetEventById(eventId).FirstOrDefault();
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            return task ?? new EventTask();
        }

        [HttpPut("AddTaskToEvent/{id}")]
        public async Task AddTask(int id, EventTask task)
        {
            mRepo.AddTask(id, task);
        }

        [HttpPut("AddAssignmentToTask/{eventId}/{taskId}")]
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            await mRepo.AddAssignmentToTask(eventId, taskId, newAssignment);
        }

        [HttpGet("GetEmployeeAssignmentsById/{UserId:int}")]
        public IEnumerable<Events> GetEmployeeAssignmentsById(int UserId)
        {
            return mRepo.GetEmployeeAssignmentsById(UserId);
        }

        [HttpGet("GetAssignmentsForTask/{eventId}/{taskId}")]
        public List<Assignment> GetAssignmentsForTask(int eventId, int taskId)
        {
            var eventItem = mRepo.GetEventById(eventId).FirstOrDefault();
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            return task?.AssignmentList ?? new List<Assignment>();
        }

        [HttpPut("UpdateEvent")]
        public async Task UpdateItem(Events product)
        {
            mRepo.UpdateItem(product);
        }

        [HttpPut("UpdateTask/{eventId}")]
        public async Task UpdateTask(int eventId, EventTask updatedTask)
        {
            var existingEvent = mRepo.GetEventById(eventId).FirstOrDefault();
            var taskIndex = existingEvent?.TaskList.FindIndex(t => t.Id == updatedTask.Id) ?? -1;

            if (existingEvent != null && taskIndex >= 0)
            {
                existingEvent.TaskList[taskIndex] = updatedTask;
                await mRepo.UpdateItem(existingEvent);
            }
        }

        [HttpDelete("DeleteEvent/{id:int}")]
        public async Task DeleteEvent(int id)
        {
            mRepo.DeleteEvent(id);
        }
    }
}
