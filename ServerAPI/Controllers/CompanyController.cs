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
        private readonly ICompanyRepository companyRepo;
        private readonly ILogger<CompaniesController> _logger;  // Logger injektion

        // Konstruktør
        public CompaniesController(ICompanyRepository repo, ILogger<CompaniesController> logger)
        {
            companyRepo = repo;
            _logger = logger; // Logger initialisering
        }

        // POST /api/companies/add
        [HttpPost("add")]
        public IActionResult AddCompany([FromBody] Company newCompany)
        {
            if (newCompany == null)
            {
                _logger.LogError("Received null company.");
                return BadRequest("Company cannot be null.");
            }

            try
            {
                // Tilføjer virksomhed til databasen
                companyRepo.AddCompany(newCompany);
                _logger.LogInformation($"Company {newCompany.Name} added successfully with ID {newCompany.Id}");

                return Ok(new { message = "Company added successfully", companyId = newCompany.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add company.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET /api/companies
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            try
            {
                var companies = companyRepo.GetAllCompanies();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve companies.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
