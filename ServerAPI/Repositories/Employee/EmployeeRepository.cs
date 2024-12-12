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
        
        public void UpdateEmployee(Employee employee)
        {
            // Create the update definition for updating the employee's fields
            var updateDef = Builders<Employee>.Update
                .Set(x => x.Name, employee.Name)
                .Set(x => x.Email, employee.Email) // Assuming employee has Position field
                .Set(x => x.Password, employee.Password) // Assuming employee has Salary field
                .Set(x => x.Number, employee.Number);       // Assuming employee has Status field
    
            // Perform the update operation in MongoDB
            var result = _collection.UpdateOne(
                x => x.Id == employee.Id, // Match the employee by Id
                updateDef);                // Apply the update definition

            if (result.MatchedCount == 0)
            {
                // Handle the case where no document was matched
                Console.WriteLine("No matching employee found to update.");
            }
            else
            {
                // Successfully updated the employee
                Console.WriteLine($"Employee {employee.Id} updated successfully.");
            }
        }
        public async Task<bool> UpdateEmployeeRole(int employeeId, int newRole)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);

            var updateDef = Builders<Employee>.Update.Set(e => e.Role, newRole);
            var result = await _collection.UpdateOneAsync(filter, updateDef);
            
            if (result.MatchedCount > 0)
            {
                Console.WriteLine($"Employee {employeeId} role updated to {newRole}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Employee {employeeId} not found.");
                return false;
            }
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

        public Employee? GetById(int UserId)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, UserId);
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
