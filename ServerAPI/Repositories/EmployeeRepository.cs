using MongoDB.Driver;
using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
    public class CustomersRepository
    {
        private string connectionString = "mongodb+srv://fguldbaek:EmX759ivZyR6VZgD@cluster0.ravrm.mongodb.net/";

        IMongoClient mongoClient;

        IMongoDatabase database;

        IMongoCollection<Employee> collection;

        public CustomersRepository()
        {
            mongoClient = new MongoClient(connectionString);

            database = mongoClient.GetDatabase("OrdersDB");

            collection = database.GetCollection<Employee>("Employee");
        }
    }
}