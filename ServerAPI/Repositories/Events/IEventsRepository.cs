using Core.Models;

namespace ServerAPI.Repositories;

public interface IEventsRepository
{
    
    void AddEvent(Events newEvent);
    
    Events[] GetAllEvents();
    
    Events[] GetEventById(int id);

    public Events[] GetEmployeeAssignmentsById(int userId);

    Task UpdateItem(Events item);
    
    Task AddTask(int id, EventTask item); // This should be async

    Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment);

    Task DeleteEvent(int eventId);

}