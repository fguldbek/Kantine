using Microsoft.AspNetCore.Mvc;
using Core.Models; 
using ServerAPI.Repositories; 

namespace ServerAPI.Controllers
{

    [ApiController]
    // Angiver base-ruten for alle endpoints i denne controller
    [Route("api/communication")]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationRepository _repo;
        public CommunicationController(ICommunicationRepository repo)
        {
            _repo = repo;
        }

        // Endpoint til at sende en besked
        // HTTP POST-metode
        [HttpPost("sendmessage")]
        public string SendMessage(Communication newMessage)
        {
            _repo.SendMessage(newMessage);
            return ("Besked sendt med succes."); // Returnerer en succesmeddelelse til klienten
        }

        // Endpoint til at hente alle beskeder, der er knyttet til en bestemt bruger
        // HTTP GET-metode, som modtager en UserId som parameter
        [HttpGet("GetAllMessageWithID/{UserId:int}")]
        public IEnumerable<Communication> GetAllMessageWithID(int UserId)
        {
            return _repo.GetAllMessageWithID(UserId); // Henter alle beskeder for den angivne bruger fra repository
        }
    }
}