using System;
using Core;
using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{

    public interface ILoginRepository
    {
        Task<Employee?> GetEmployeeByCredentialsAsync(string username, string password);
    }
}