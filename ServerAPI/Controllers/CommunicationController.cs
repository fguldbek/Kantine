using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;
using Microsoft.Extensions.Logging;  

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/communication")]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationRepository _repo;
        private readonly ILogger<CommunicationController> _logger;  // Logger injektion

        // Konstruktør
        public CommunicationController(ICommunicationRepository repo, ILogger<CommunicationController> logger)
        {
            _repo = repo;
            _logger = logger; // Logger initialisering
        }
        
        [HttpPost("sendmessage")]
        public IActionResult SendMessage(Communication newMessage)
        {
            try
            {
                _repo.SendMessage(newMessage);
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee.");
                return StatusCode(500, "Internal server error.");
            }
        }
        
        [HttpGet]
        [Route("GetAllMessageWithID/{UserId:int}")]
        public IEnumerable<Communication> GetAllMessageWithID(int UserId){
                return _repo.GetAllMessageWithID(UserId); 
        }

        
    }
}
