using System;
using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
	public class EventsRepository : IEventsRepository
	{
        private IMongoClient client;
        private IMongoCollection<Events> collection;

        public EventsRepository()
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
            var collectionName = "Events";

            collection = client.GetDatabase(dbName)
               .GetCollection<Events>(collectionName);

        }
        public void AddEvent(Events item)
        {
            var max = 0;
            if (collection.Count(Builders<Events>.Filter.Empty) > 0)
            {
                max = collection.Find(Builders<Events>.Filter.Empty).SortByDescending(r => r.Id).Limit(1).ToList()[0].Id;
            }
            item.Id = max + 1;
            collection.InsertOne(item);
           
        }
    }
}
