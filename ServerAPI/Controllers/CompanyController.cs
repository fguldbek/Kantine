using Microsoft.AspNetCore.Mvc;
using Core.Models; 
using ServerAPI.Repositories; 


namespace ServerAPI.Controllers
{
    [ApiController]
    // Angiver base-ruten for alle endpoints i denne controller
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repo;
        
        public CompaniesController(ICompanyRepository repo)
        {
            _repo = repo; // Initialiserer repository med det indsprøjtede repository-objekt
        }

        // Endpoint til at tilføje en ny virksomhed
        // HTTP POST-metode
        [HttpPost("add")]
        public async Task AddCompany(Company newCompany)
        {
            // Tilføjer virksomheden til databasen via repository
            _repo.AddCompany(newCompany);
        }

        // Endpoint til at hente alle virksomheder
        // HTTP GET-metode uden parametre.
        [HttpGet]
        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return _repo.GetAllCompanies();
        }
    }
}
