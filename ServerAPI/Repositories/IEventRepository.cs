using Core.Models;

namespace ServerAPI.Repositories;

public interface IEventRepository
{
    
    void AddEvent(Events newEvent);
    
}