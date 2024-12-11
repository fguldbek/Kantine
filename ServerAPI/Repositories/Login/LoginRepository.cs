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
        private readonly ILogger<LoginRepository> _logger;


        public LoginRepository(ILogger<LoginRepository> logger)
        {
            _logger = logger;

            
            var mongoUri = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            try
            {
                _client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                _logger.LogError("There was a problem connecting to MongoDB Atlas. Message: {Message}", e.Message);
                throw;
            }


            var dbName = "KantineDatabase";
            var collectionName = "Employee";

            _collection = _client.GetDatabase(dbName)
                .GetCollection<Employee>(collectionName);
        }
        
        // Tilføjelse af Login-metode
        public async Task<Employee?> GetEmployeeByCredentialsAsync(string username, string password)
        {
            
                var filter = Builders<Employee>.Filter.And(
                    Builders<Employee>.Filter.Eq(e => e.Name, username),
                    Builders<Employee>.Filter.Eq(e => e.Password, password)
                );

                return await _collection.Find(filter).FirstOrDefaultAsync();
            
        }
    }
}
