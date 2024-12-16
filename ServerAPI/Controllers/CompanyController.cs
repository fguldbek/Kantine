using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerAPI.Repositories;
using Microsoft.Extensions.Logging;  // Tilføj dette for Logger

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repo;
        private readonly ILogger<CompaniesController> _logger;  // Logger injektion

        // Konstruktør
        public CompaniesController(ICompanyRepository repo, ILogger<CompaniesController> logger)
        {
            _repo = repo;
            _logger = logger; // Logger initialisering
        }
        [HttpPost("add")]
        public async Task AddCompany(Company newCompany)
        {
            if (newCompany == null)
            {
                _logger.LogError("Received null company.");
                throw new ArgumentException("Company cannot be null.");
            }

            // Tilføjer virksomhed til databasen
            _repo.AddCompany(newCompany);
            _logger.LogInformation($"Company {newCompany.Name} added successfully with ID {newCompany.Id}");
        }

        [HttpGet]
        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return _repo.GetAllCompanies();
        }

    }
}
