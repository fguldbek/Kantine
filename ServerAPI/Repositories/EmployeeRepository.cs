using System;
using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
        private IMongoClient client;
        private IMongoCollection<Employee> collection;

        public EmployeeRepository()
		{
            var password = ""; //add
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            

            try
            {
                client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                return;
            }

            // Provide the name of the database and collection you want to use.
            // If they don't already exist, the driver and Atlas will create them
            // automatically when you first write data.
            var dbName = "KantineDatabase";
            var collectionName = "Employee";

            collection = client.GetDatabase(dbName)
               .GetCollection<Employee>(collectionName);

        }

        public void Add(Employee item)
        {
            var max = 0;
            if (collection.Count(Builders<Employee>.Filter.Empty) > 0)
            {
                max = collection.Find(Builders<Employee>.Filter.Empty).SortByDescending(r => r.Id).Limit(1).ToList()[0].Id;
            }
            item.Id = max + 1;
            collection.InsertOne(item);
           
        }
        
        

        public void DeleteById(int id){
            var deleteResult = collection
                .DeleteOne(Builders<Employee>.Filter.Where(r => r.Id == id));
        }
        
        // Updated GetUserIdAsync to retrieve by Id
        public Employee[] GetAllByUserId(int Id)
        {
                // Using MongoDB dot notation to access the nested 'User.BuyerId' field in the filter
                var filter = Builders<Employee>.Filter.Eq("Id", Id);
                return collection.Find(filter).ToList().ToArray();
        }


        public Employee[] GetAll()
        {
           return collection.Find(Builders<Employee>.Filter.Empty).ToList().ToArray();
        }

        /*public ShoppingItem[] GetAllByShop(string shop)
        {
            var filter = Builders<ShoppingItem>.Filter.Where(r => r.Shop.Equals(shop));
            return collection.Find(filter).ToList().ToArray();

        }*/

        public void UpdateItem(Employee item)
        {
            var updateDef = Builders<Employee>.Update
                 .Set(x => x.Name, item.Name)
                 .Set(x => x.Password, item.Password)
                 .Set(x => x.Role, item.Role);
            collection.UpdateOne(x => x.Id == item.Id, updateDef);
        }
        
        
    }
}

