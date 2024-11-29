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
        
        Employee[] GetAllByUserId(int Id);
    }
}