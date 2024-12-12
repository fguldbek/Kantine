using System;
using Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task UpdateItem(Events item)
        {
            var filter = Builders<Events>.Filter.Eq(e => e.Id, item.Id);
            var update = Builders<Events>.Update
                .Set(e => e.Name, item.Name)
                .Set(e => e.StartDate, item.StartDate)
                .Set(e => e.Location, item.Location)
                .Set(e => e.Food, item.Food)
                .Set(e => e.Participants, item.Participants)
                .Set(e => e.Requests, item.Requests)
                .Set(e => e.Company, item.Company)
                .Set(e => e.TaskList, item.TaskList);

            await collection.UpdateOneAsync(filter, update);
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
        
        
        public Events[] GetEmployeeAssignmentsById(int userId)
        {
            var filter = Builders<Events>.Filter.ElemMatch(
                e => e.TaskList,
                Builders<EventTask>.Filter.ElemMatch(
                    t => t.AssignmentList,
                    a => a.EmployeeId == userId
                )
            );

            return collection.Find(filter).ToList().ToArray();
        }
        
        public async Task UpdateEmployeeAssignmentsById(int userId, bool newStatus, int newHoursUsed)
        {
            // Define the filter to locate the specific event, task, and assignment
            var filter = Builders<Events>.Filter.ElemMatch(
                e => e.TaskList,
                Builders<EventTask>.Filter.ElemMatch(
                    t => t.AssignmentList,
                    a => a.EmployeeId == userId
                )
            );

            // Define the update operation for the matched elements
            var update = Builders<Events>.Update
                .Set("TaskList.$[task].AssignmentList.$[assignment].Status", newStatus)
                .Set("TaskList.$[task].AssignmentList.$[assignment].HoursUsed", newHoursUsed);

            // Specify array filters to target the correct nested elements
            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new JsonArrayFilterDefinition<EventTask>("{ 'task.AssignmentList.EmployeeId': " + userId + " }"),
                new JsonArrayFilterDefinition<Assignment>("{ 'assignment.EmployeeId': " + userId + " }")
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            // Perform the update operation
            await collection.UpdateManyAsync(filter, update, updateOptions);
        }


        
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            // Find the event and the task within the event
            var filter = Builders<Events>.Filter.And(
                Builders<Events>.Filter.Eq(e => e.Id, eventId),
                Builders<Events>.Filter.ElemMatch(e => e.TaskList, t => t.Id == taskId)
            );

            // Get the existing event with task details
            var eventItem = await collection.Find(filter).FirstOrDefaultAsync();
            
            // Tjekker om der er nogle tasks, hvis der er nogle task
            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId);

            // Generate a new unique ID for the assignment
            int newAssignmentId = 1; // Default ID
            if (task.AssignmentList != null && task.AssignmentList.Count > 0)
            {
                newAssignmentId = task.AssignmentList.Max(a => a.Id) + 1;
            }

            // Set the ID of the new assignment
            newAssignment.Id = newAssignmentId;

            var update = Builders<Events>.Update.Push("TaskList.$.AssignmentList", newAssignment);
            await collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteEvent(int eventId)
        {
            var deleteResult = await collection.DeleteOneAsync(Builders<Events>.Filter.Eq(r => r.Id, eventId));

            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"No item found with ID {eventId} to delete.");
            }
        }
    }

    }
    

