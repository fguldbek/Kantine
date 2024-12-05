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
            var password = ""; // Tilføj din adgangskode her
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            try
            {
                client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                Console.WriteLine("Der opstod et problem med at oprette forbindelse til din Atlas-kluster. Kontroller, at URI'en inkluderer et gyldigt brugernavn og adgangskode, og at din IP-adresse er på adgangspunktet. Fejlmeddelelse: " + e.Message);
                Console.WriteLine(e);
                return;
            }

            var dbName = "KantineDatabase";
            var collectionName = "Events";
            collection = client.GetDatabase(dbName).GetCollection<Events>(collectionName);
        }

        
        public void AddEvent(Events item)
        {
            // Generering af ID (kan også gøres af MongoDB selv, men her er en manuel metode)
            var max = 0;
            if (collection.Count(Builders<Events>.Filter.Empty) > 0)
            {
                max = collection.Find(Builders<Events>.Filter.Empty)
                    .SortByDescending(r => r.Id)
                    .Limit(1)
                    .ToList()[0].Id;
            }
            item.Id = max + 1;  // Tildeler et nyt ID til eventet

            // Insert eventet sammen med det indlejrede Company
            collection.InsertOne(item);
        }
        
        
        public Events[] GetAllEvents()
        {
                var orders = collection.Find(Builders<Events>.Filter.Empty).ToList().ToArray();
                return orders;
        }

        public Events[] GetEventById(int id)
        {
            var filter = Builders<Events>.Filter.Eq("Id", id);
            return collection.Find(filter).ToList().ToArray();
        }
        
      
        public EventTask GetEventTaskById(int eventId, int taskId)
        {
            var filter = Builders<Events>.Filter.Eq(e => e.Id, eventId);

            var projection = Builders<Events>.Projection.Expression(
                e => e.TaskList.FirstOrDefault(t => t.Id == taskId)
            );

            var result = collection.Find(filter)
                .Project(projection)
                .FirstOrDefault();

            return result;
        }
        public void UpdateItem(Events item)
        { 
            var filter = Builders<Events>.Filter.Eq(x => x.Id, item.Id);

            var updateDef = Builders<Events>.Update

                .Set(x => x.Name, item.Name)
                .Set(x => x.StartDate, item.StartDate)
                .Set(x => x.Location, item.Location)
                .Set(x => x.Food, item.Food)
                .Set(x => x.Participants, item.Participants)
                .Set(x => x.Requests, item.Requests)
                .Set(x => x.Company, item.Company)
                .Set(x => x.TaskList, item.TaskList);
            collection.UpdateOne(filter, updateDef);
        }
        
        public async Task AddTask(int eventId, EventTask item)
        {
            // Find the event to get the existing tasks
            var filter = Builders<Events>.Filter.Eq(e => e.Id, eventId);
            var eventItem = collection.Find(filter).FirstOrDefault();

           
            if (eventItem.TaskList == null)
            {
                eventItem.TaskList = new List<EventTask>();
            }

            // Generate a new unique ID for the task
            int newTaskId = 1; // Default ID
            if (eventItem.TaskList != null && eventItem.TaskList.Count > 0)
            {
                newTaskId = eventItem.TaskList.Max(t => t.Id) + 1;
            }

            // Set the ID of the new task
            item.Id = newTaskId;

            // Push the task to the TaskList in the database
            var updateDef = Builders<Events>.Update.Push(e => e.TaskList, item);
            await collection.UpdateOneAsync(filter, updateDef);
        }
        
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            var filter = Builders<Events>.Filter.And(
                Builders<Events>.Filter.Eq(e => e.Id, eventId),
                Builders<Events>.Filter.ElemMatch(e => e.TaskList, t => t.Id == taskId)
            );

            var update = Builders<Events>.Update.Push("TaskList.$.AssignmentList", newAssignment);

            await collection.UpdateOneAsync(filter, update);
        }



    }

    }
    

