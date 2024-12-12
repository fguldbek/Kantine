using System;
using Core;
using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
    /*
     * Repræsenterer en samling af ShoppingItems.
     */
    public interface IEmployeeRepository
    {
        //Tildeler item en unik id og tilføjer den.
        void Add(Employee item);

        public void UpdateEmployee(Employee employee);

        Employee? GetById(int UserId);
        
        IEnumerable<Employee> GetAllEmployees();
        
        Task<bool> UpdateEmployeeRole(int employeeId, int newRole);

    }
}