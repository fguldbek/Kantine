using Core.Models;
using System.Collections.Generic;

namespace ServerAPI.Repositories
{
    public interface ICompanyRepository
    {
        void AddCompany(Company newCompany);
        IEnumerable<Company> GetAllCompanies();
    }
}