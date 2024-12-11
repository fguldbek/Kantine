using Core.Models;

namespace ServerAPI.Repositories;

public interface ICommunicationRepository
{
    
    void SendMessage(Communication newMessage);

    public Communication[] GetAllMessageWithID(int UserId);
}