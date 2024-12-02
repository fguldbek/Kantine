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
    public class EventTaskRepository
    {
        private string connectionString = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

        IMongoClient mongoClient;

        IMongoDatabase database;

        IMongoCollection<Employee> collection;

        public EventTaskRepository()
        {
            mongoClient = new MongoClient(connectionString);

            database = mongoClient.GetDatabase("KantineDatabase");

            collection = database.GetCollection<Employee>("Task");
        }
    }
}