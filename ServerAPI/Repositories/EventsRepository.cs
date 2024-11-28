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
    public class EventsRepository
    {
        private string connectionString = "mongodb+srv://fguldbaek:EmX759ivZyR6VZgD@cluster0.ravrm.mongodb.net/";

        IMongoClient mongoClient;

        IMongoDatabase database;

        IMongoCollection<Employee> collection;

        public EventsRepository()
        {
            mongoClient = new MongoClient(connectionString);

            database = mongoClient.GetDatabase("KantineDatabase");

            collection = database.GetCollection<Employee>("Events");
        }
    }
}