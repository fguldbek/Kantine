using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Models;

namespace ServerAPI.Repositories
{
    public class EventsRepository : IEventRepository
    {
        private string connectionString = "mongodb+srv://fguldbaek:EmX759ivZyR6VZgD@cluster0.ravrm.mongodb.net/";

        private readonly IMongoClient mongoClient;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Events> collection;

        public EventsRepository()
        {
            mongoClient = new MongoClient(connectionString);
            database = mongoClient.GetDatabase("KantineDatabase");
            collection = database.GetCollection<Events>("Events");
        }

        public void AddEvent(Events newEvent)
        {
            try
            {
                collection.InsertOne(newEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding an event: {ex.Message}");
                throw;
            }
        
        }
    }
}
