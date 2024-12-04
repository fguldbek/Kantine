using Core.Models;

namespace ServerAPI.Repositories;

public interface IEventsRepository
{
    
    void AddEvent(Events newEvent);
    
    Events[] GetAllEvents();
    
    Events[] GetEventById(int id);

    public void UpdateItem(Events item);
    
    public void AddTask(Events item);



}