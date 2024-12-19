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
            // MongoDB URI, som indeholder forbindelsesstrengen til database
            var mongoUri = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/"; 
            
            // Opretter en forbindelse til MongoDB
            _client = new MongoClient(mongoUri); 
            
            
            var dbName = "KantineDatabase";  // Navn på databasen
            var collectionName = "Employee";  // Navn på collection

            // Opretter forbindelse til den specifikke collection
            _collection = _client.GetDatabase(dbName)
                .GetCollection<Employee>(collectionName);  // Henter collection 'Employee'
        }
        
        // Metode til at opdatere en medarbejders oplysninger
        public void UpdateEmployee(Employee employee)
        {
            // Opretter en opdatering, der ændrer medarbejderens navn, email, password, og nummer
            var updateDef = Builders<Employee>.Update
                .Set(x => x.Name, employee.Name)
                .Set(x => x.Email, employee.Email)
                .Set(x => x.Password, employee.Password)
                .Set(x => x.Number, employee.Number);
    
            // Udfører opdateringen i MongoDB baseret på medarbejderens ID
            var result = _collection.UpdateOne(
                x => x.Id == employee.Id,  // Finder den medarbejder der matcher ID'et
                updateDef);                 // Anvender opdateringens definition
        }

        // Asynkron metode til at opdatere en medarbejders rolle
        public async Task<bool> UpdateEmployeeRole(int employeeId, int newRole)
        {
            var filter = Builders<Employee>.Filter.Eq(employee => employee.Id, employeeId); // Filtrerer medarbejderen baseret på ID

            var updateDef = Builders<Employee>.Update.Set(employee => employee.Role, newRole); // Sætter den nye rolle
            var result = await _collection.UpdateOneAsync(filter, updateDef); // Udfører den asynkrone opdatering

            // Tjekker om opdateringen blev gennemført og logger resultatet
            if (result.MatchedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Metode til at tilføje en medarbejder til databasen
        public void Add(Employee item)
        {
            var max = 0;
            // Hvis der allerede findes medarbejdere i databasen, finder vi den højeste ID-værdi
            if (_collection.CountDocuments(Builders<Employee>.Filter.Empty) > 0)
            {
                max = _collection.Find(Builders<Employee>.Filter.Empty)
                    .SortByDescending(r => r.Id)  // Sorterer efter ID i faldende rækkefølge
                    .Limit(1)  // Begrænser resultatet til 1 (det højeste ID)
                    .First()  // Henter den første (nyeste) medarbejder
                    .Id;  // Henter ID'et på denne medarbejder
            }

            // Sætter ID'et for den nye medarbejder (maksimalt ID + 1)
            item.Id = max + 1;

            // Indsætter den nye medarbejder i databasen
            _collection.InsertOne(item);
        }

        // Metode til at hente en medarbejder baseret på ID
        public Employee? GetById(int UserId)
        {
            var filter = Builders<Employee>.Filter.Eq(employee => employee.Id, UserId); // Filtrerer efter medarbejderens ID
            return _collection.Find(filter).FirstOrDefault(); // Returnerer den første medarbejder der matcher ID'et
        }
        
        // Metode til at slette en medarbejder baseret på ID
        public async Task DeleteEmployeeById(int id)
        {
            await _collection.DeleteOneAsync(Builders<Employee>.Filter.Eq(employee => employee.Id, id));
        }

        
        // Metode til at hente alle medarbejdere
        public IEnumerable<Employee> GetAllEmployees()
        {
            // Henter og returnerer alle medarbejdere fra databasen
            return _collection.Find(Builders<Employee>.Filter.Empty).ToList(); 
        }
    }
}
