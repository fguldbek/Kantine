using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _repo;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventsRepository repo, ILogger<EventsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task AddEvent(Events newEvent)
        {
            _repo.AddEvent(newEvent);
        }

        [HttpGet("GetAllEvents")]
        public IEnumerable<Events> GetAllEvents()
        {
            return _repo.GetAllEvents();
        }

        [HttpGet("GetEventById/{id}")]
        public Events GetEventById(int id)
        {
            var eventItem = _repo.GetEventById(id).FirstOrDefault();
            return eventItem ?? new Events();
        }

        [HttpGet("GetEventTaskById/{eventId}/{taskId}")]
        public EventTask GetEventTaskById(int eventId, int taskId)
        {
            var eventItem = _repo.GetEventById(eventId).FirstOrDefault();
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            return task ?? new EventTask();
        }

        [HttpPut("AddTaskToEvent/{id}")]
        public async Task AddTask(int id, EventTask task)
        {
            _repo.AddTask(id, task);
        }

        [HttpPut("AddAssignmentToTask/{eventId}/{taskId}")]
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            await _repo.AddAssignmentToTask(eventId, taskId, newAssignment);
        }

        [HttpGet("GetEmployeeAssignmentsById/{UserId:int}")]
        public IEnumerable<Events> GetEmployeeAssignmentsById(int UserId)
        {
            return _repo.GetEmployeeAssignmentsById(UserId);
        }

        [HttpGet("GetAssignmentsForTask/{eventId}/{taskId}")]
        public List<Assignment> GetAssignmentsForTask(int eventId, int taskId)
        {
            var eventItem = _repo.GetEventById(eventId).FirstOrDefault();
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            return task?.AssignmentList ?? new List<Assignment>();
        }

        [HttpPut("UpdateEvent")]
        public async Task UpdateItem(Events product)
        {
            _repo.UpdateItem(product);
        }

        [HttpPut("UpdateTask/{eventId}")]
        public async Task UpdateTask(int eventId, EventTask updatedTask)
        {
            var existingEvent = _repo.GetEventById(eventId).FirstOrDefault();
            var taskIndex = existingEvent?.TaskList.FindIndex(t => t.Id == updatedTask.Id) ?? -1;

            if (existingEvent != null && taskIndex >= 0)
            {
                existingEvent.TaskList[taskIndex] = updatedTask;
                await _repo.UpdateItem(existingEvent);
            }
        }

        [HttpDelete("DeleteEvent/{id:int}")]
        public async Task DeleteEvent(int id)
        {
            _repo.DeleteEvent(id);
        }
    }
}
