using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/events")] // Definerer grundruten for alle endpoints i denne controller
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _repo; // Interface til Event Repository
        
        public EventsController(IEventsRepository repo)
        {
            _repo = repo;
        }

        // Endpoint til at tilføje et nyt event
        [HttpPost("add")]
        public async Task AddEvent(Events newEvent)
        {
            // Tilføjer et nyt event via repository
            _repo.AddEvent(newEvent);
        }

        // Endpoint til at hente alle events
        [HttpGet("GetAllEvents")]
        public IEnumerable<Events> GetAllEvents()
        {
            // Returnerer alle events fra repository
            return _repo.GetAllEvents();
        }

        // Endpoint til at hente et specifikt event baseret på event ID
        [HttpGet("GetEventById/{id}")]
        public Events GetEventById(int id)
        {
            // Henter event baseret på ID, returnerer tom event hvis ikke fundet
            var eventItem = _repo.GetEventById(id).FirstOrDefault();
            return eventItem ?? new Events(); // Returnerer en tom Events objekt hvis ikke fundet
        }

        // Endpoint til at hente en specifik opgave for et event baseret på eventId og taskId
        [HttpGet("GetEventTaskById/{eventId}/{taskId}")]
        public EventTask GetEventTaskById(int eventId, int taskId)
        {
            // Henter event baseret på ID
            var eventItem = _repo.GetEventById(eventId).FirstOrDefault();
            // Henter task fra event baseret på taskId
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            return task ?? new EventTask(); // Returnerer en tom EventTask hvis ikke fundet
        }

        // Endpoint til at tilføje en ny opgave til et event
        [HttpPut("AddTaskToEvent/{id}")]
        public async Task AddTask(int id, EventTask task)
        {
            // Tilføjer en opgave til et event via repository
            _repo.AddTask(id, task);
        }

        // Endpoint til at tilføje en assignment til en opgave på et event
        [HttpPut("AddAssignmentToTask/{eventId}/{taskId}")]
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            // Tilføjer assignment til task via repository
            await _repo.AddAssignmentToTask(eventId, taskId, newAssignment);
        }

        // Endpoint til at hente alle events for en medarbejder baseret på UserId
        [HttpGet("GetEmployeeAssignmentsById/{UserId:int}")]
        public IEnumerable<Events> GetEmployeeAssignmentsById(int UserId)
        {
            // Henter events for en medarbejder baseret på UserId
            return _repo.GetEmployeeAssignmentsById(UserId);
        }

        // Endpoint til at hente assignments (opgaveansvar) for en specifik opgave i et event
        [HttpGet("GetAssignmentsForTask/{eventId}/{taskId}")]
        public List<Assignment> GetAssignmentsForTask(int eventId, int taskId)
        {
            // Henter event baseret på eventId
            var eventItem = _repo.GetEventById(eventId).FirstOrDefault();
            // Henter task fra event baseret på taskId
            var task = eventItem?.TaskList.FirstOrDefault(t => t.Id == taskId);
            // Returnerer assignments (opgaveansvar) for den opgave
            return task?.AssignmentList ?? new List<Assignment>(); // Returnerer tom liste hvis ikke fundet
        }

        // Endpoint til at opdatere et event
        [HttpPut("UpdateEvent")]
        public async Task UpdateItem(Events product)
        {
            // Opdaterer event via repository
            _repo.UpdateItem(product);
        }

        // Endpoint til at opdatere en opgave på et event
        [HttpPut("UpdateTask/{eventId}")]
        public async Task UpdateTask(int eventId, EventTask updatedTask)
        {
            // Henter event baseret på eventId
            var existingEvent = _repo.GetEventById(eventId).FirstOrDefault();
            // Finder index for tasken i eventets TaskList
            var taskIndex = existingEvent?.TaskList.FindIndex(t => t.Id == updatedTask.Id) ?? -1;

            // Hvis event og task findes, opdaterer tasken
            if (existingEvent != null && taskIndex >= 0)
            {
                existingEvent.TaskList[taskIndex] = updatedTask;
                // Opdaterer event via repository
                await _repo.UpdateItem(existingEvent);
            }
        }

        // Endpoint til at slette et event baseret på ID
        [HttpDelete("DeleteEvent/{id:int}")]
        public async Task DeleteEvent(int id)
        {
            // Sletter event via repository
            _repo.DeleteEvent(id);
        }
    }
}
