using System;
using Core;
using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{

    public interface IEmployeeRepository
    {
        void Add(Employee item);

        public void UpdateEmployee(Employee employee);

        Employee? GetById(int UserId);
        
        IEnumerable<Employee> GetAllEmployees();
        
        Task<bool> UpdateEmployeeRole(int employeeId, int newRole);

        Task DeleteEmployeeById(int id);

    }
}