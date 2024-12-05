using System;
using System.Threading.Tasks;
using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IMongoClient _client;
        private IMongoCollection<Employee> _collection;

        public EmployeeRepository()
        {
            var mongoUri = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            try
            {
                _client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem connecting to your Atlas cluster.");
                Console.WriteLine($"Message: {e.Message}");
                throw;
            }

            var dbName = "KantineDatabase";
            var collectionName = "Employee";

            _collection = _client.GetDatabase(dbName)
                .GetCollection<Employee>(collectionName);
        }

        public void Add(Employee item)
        {
            var max = 0;
            if (_collection.CountDocuments(Builders<Employee>.Filter.Empty) > 0)
            {
                max = _collection.Find(Builders<Employee>.Filter.Empty)
                    .SortByDescending(r => r.Id)
                    .Limit(1)
                    .First()
                    .Id;
            }
            item.Id = max + 1;
            _collection.InsertOne(item);
        }

        public void DeleteById(int id)
        {
            _collection.DeleteOne(Builders<Employee>.Filter.Where(r => r.Id == id));
        }

        public Employee? GetById(int id)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }
        
     
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _collection.Find(Builders<Employee>.Filter.Empty).ToList();
        }

        public void UpdateItem(Employee item)
        {
            var updateDef = Builders<Employee>.Update
                .Set(x => x.Name, item.Name)
                .Set(x => x.Password, item.Password)
                .Set(x => x.Role, item.Role);
            _collection.UpdateOne(x => x.Id == item.Id, updateDef);
        }
    }
}
