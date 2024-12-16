using System;
using System.Threading.Tasks;
using Core.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace ServerAPI.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private IMongoClient client;
        private IMongoCollection<Events> collection;

        public EventsRepository()
        {
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            // Opretter en forbindelse til MongoDB uden try-catch
            client = new MongoClient(mongoUri);

            var dbName = "KantineDatabase";
            var collectionName = "Events";
            collection = client.GetDatabase(dbName).GetCollection<Events>(collectionName);
        }

        public void AddEvent(Events item)
        {
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
            var filter = Builders<Events>.Filter.Eq(e => e.Id, eventId);
            var eventItem = collection.Find(filter).FirstOrDefault();

            if (eventItem.TaskList == null)
            {
                eventItem.TaskList = new List<EventTask>();
            }

            int newTaskId = 1; // Default ID
            if (eventItem.TaskList != null && eventItem.TaskList.Count > 0)
            {
                newTaskId = eventItem.TaskList.Max(t => t.Id) + 1;
            }

            item.Id = newTaskId;

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
            var filter = Builders<Events>.Filter.ElemMatch(
                e => e.TaskList,
                Builders<EventTask>.Filter.ElemMatch(
                    t => t.AssignmentList,
                    a => a.EmployeeId == userId
                )
            );

            var update = Builders<Events>.Update
                .Set("TaskList.$[task].AssignmentList.$[assignment].Status", newStatus)
                .Set("TaskList.$[task].AssignmentList.$[assignment].HoursUsed", newHoursUsed);

            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new JsonArrayFilterDefinition<EventTask>("{ 'task.AssignmentList.EmployeeId': " + userId + " }"),
                new JsonArrayFilterDefinition<Assignment>("{ 'assignment.EmployeeId': " + userId + " }")
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            await collection.UpdateManyAsync(filter, update, updateOptions);
        }

        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            var filter = Builders<Events>.Filter.And(
                Builders<Events>.Filter.Eq(e => e.Id, eventId),
                Builders<Events>.Filter.ElemMatch(e => e.TaskList, t => t.Id == taskId)
            );

            var eventItem = await collection.Find(filter).FirstOrDefaultAsync();
            if (eventItem == null)
            {
                return; // Event or Task not found, no exception thrown
            }

            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return; // Task not found, no exception thrown
            }

            int newAssignmentId = 1; // Default ID
            if (task.AssignmentList != null && task.AssignmentList.Count > 0)
            {
                newAssignmentId = task.AssignmentList.Max(a => a.Id) + 1;
            }

            newAssignment.Id = newAssignmentId;

            var update = Builders<Events>.Update.Push("TaskList.$.AssignmentList", newAssignment)
                .Inc("TaskList.$.EmployeesAssigned", 1);
            await collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteEvent(int eventId)
        {
            await collection.DeleteOneAsync(Builders<Events>.Filter.Eq(r => r.Id, eventId));
        }
    }
}


