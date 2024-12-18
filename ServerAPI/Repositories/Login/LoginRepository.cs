using System;
using System.Threading.Tasks;
using Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace ServerAPI.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private IMongoClient _client; 
        private IMongoCollection<Employee> _collection; 
        

        public LoginRepository(ILogger<LoginRepository> logger)
        {

            // MongoDB URI, som indeholder forbindelsesstrengen til database
            var mongoUri = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";
            
            // Opretter en forbindelse til MongoDB
            _client = new MongoClient(mongoUri);
            
            var dbName = "KantineDatabase"; // Navnet på databasen
            var collectionName = "Employee"; // Navn på collection

            // Opretter forbindelse til den specifikke collection
            _collection = _client.GetDatabase(dbName)
                .GetCollection<Employee>(collectionName);// Henter den ønskede collection (Employee)
        }
        
        // Asynkron metode til at hente medarbejderen baseret på loginoplysninger
        public async Task<Employee?> GetEmployeeByCredentialsAsync(string username, string password)
        {
            // Opretter et filter for at finde medarbejderen med det angivne brugernavn og password
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(e => e.Name, username), // Filtrerer på brugerens username
                Builders<Employee>.Filter.Eq(e => e.Password, password) // Filtrerer på brugerens password
            );

            // Udfører en asynkron forespørgsel til databasen og returnerer den første medarbejder, der matcher filteret
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
