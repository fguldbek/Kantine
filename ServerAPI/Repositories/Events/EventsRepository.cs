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
            // MongoDB URI, som indeholder forbindelsesstrengen til database
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            // Opretter en forbindelse til MongoDB
            client = new MongoClient(mongoUri);

            var dbName = "KantineDatabase"; // Navn på databasen
            var collectionName = "Events"; // Navn på collection
            
            // Opretter forbindelse til den specifikke collection
            collection = client.GetDatabase(dbName)
                .GetCollection<Events>(collectionName); // Henter den ønskede collection (Events)
        }

        // Tilføjer et nyt event til databasen
        public void AddEvent(Events item)
        {
            var max = 0;
            // Tjekker om der er nogen events i samlingen
            if (collection.Count(Builders<Events>.Filter.Empty) > 0)
            {
                max = collection.Find(Builders<Events>.Filter.Empty)
                    .SortByDescending(r => r.Id) // Sorterer efter ID i faldende rækkefølge
                    .Limit(1) // Begrænser resultatet til én post
                    .ToList()[0].Id; // Henter det højeste ID
            }
            item.Id = max + 1;  // Tildeler et nyt ID til eventet

            // Indsætter eventet i samlingen
            collection.InsertOne(item);
        }

        // Henter alle events fra databasen
        public Events[] GetAllEvents()
        {
            var orders = collection.Find(Builders<Events>.Filter.Empty).ToList().ToArray();
            return orders;
        }

        // Henter et event baseret på ID
        public Events[] GetEventById(int id)
        {
            var filter = Builders<Events>.Filter.Eq("Id", id); // Filtrerer events baseret på ID
            return collection.Find(filter).ToList().ToArray(); // Returnerer eventet som en array
        }

        // Opdaterer et event med nye oplysninger
        public async Task UpdateItem(Events item)
        {
            var filter = Builders<Events>.Filter.Eq(e => e.Id, item.Id); // Filtrerer eventet baseret på ID
            var update = Builders<Events>.Update
                .Set(e => e.Name, item.Name) // Opdaterer eventnavnet
                .Set(e => e.StartDate, item.StartDate) // Opdaterer startdato
                .Set(e => e.Location, item.Location) // Opdaterer lokationen
                .Set(e => e.Food, item.Food) // Opdaterer madvalget
                .Set(e => e.Participants, item.Participants) // Opdaterer deltagerlisten
                .Set(e => e.Requests, item.Requests) // Opdaterer requests
                .Set(e => e.Company, item.Company) // Opdaterer tilknyttet virksomhed
                .Set(e => e.TaskList, item.TaskList); // Opdaterer opgavelisten

            // Udfører opdateringen
            await collection.UpdateOneAsync(filter, update);
        }

        // Tilføjer en opgave til et event
        public async Task AddTask(int eventId, EventTask item)
        {
            var filter = Builders<Events>.Filter.Eq(e => e.Id, eventId); // Filtrerer eventet baseret på ID
            var eventItem = collection.Find(filter).FirstOrDefault(); // Henter eventet

            // Hvis eventet ikke har nogen TaskList, oprettes en
            if (eventItem.TaskList == null)
            {
                eventItem.TaskList = new List<EventTask>();
            }

            int newTaskId = 1; // Standard ID for en ny opgave
            // Finder det højeste task ID og opretter et nyt ID
            if (eventItem.TaskList != null && eventItem.TaskList.Count > 0)
            {
                newTaskId = eventItem.TaskList.Max(t => t.Id) + 1;
            }

            item.Id = newTaskId; // Tildeler opgaven et ID

            var updateDef = Builders<Events>.Update.Push(e => e.TaskList, item); // Tilføjer opgaven til TaskList
            await collection.UpdateOneAsync(filter, updateDef); // Udfører opdateringen
        }
        
       // Henter events, hvor en bestemt medarbejder er tildelt en opgave baseret på userId
        public Events[] GetEmployeeAssignmentsById(int userId)
        {
            // Opretter et filter, der finder events, hvor tasklist indeholder EventTask, 
            // som igen indeholder AssignmentList for den specifikke medarbejder baseret på EmployeeId
            var filter = Builders<Events>.Filter.ElemMatch(
                e => e.TaskList, // Filtrerer events, der indeholder en TaskList
                Builders<EventTask>.Filter.ElemMatch( // Filtrerer hver task i tasklisten
                    t => t.AssignmentList, // Filtrerer opgavens tildelinger (AssignmentList)
                    a => a.EmployeeId == userId // Filtrerer tildelinger baseret på userId
                )
            );

            // Finder alle events, der matcher filteret, og returnerer dem som en array af Events
            return collection.Find(filter).ToList().ToArray();
        }

        
        // Tilføjer en Assignment til en Task i et event
        public async Task AddAssignmentToTask(int eventId, int taskId, Assignment newAssignment)
        {
            var filter = Builders<Events>.Filter.And(
                Builders<Events>.Filter.Eq(e => e.Id, eventId), // Filtrerer baseret på event ID
                Builders<Events>.Filter.ElemMatch(e => e.TaskList, t => t.Id == taskId) // Filtrerer baseret på opgave ID
            );

            var eventItem = await collection.Find(filter).FirstOrDefaultAsync(); // Henter eventet

            var task = eventItem.TaskList.FirstOrDefault(t => t.Id == taskId); // Henter opgaven

            int newAssignmentId = 1; // Standard ID for tildelingen
            // Finder det højeste assignment ID og opretter et nyt ID
            if (task.AssignmentList != null && task.AssignmentList.Count > 0)
            {
                newAssignmentId = task.AssignmentList.Max(a => a.Id) + 1;
            }

            newAssignment.Id = newAssignmentId; // Tildeler assignment et ID

            // Opdaterer eventet med den nye tildeling og øger antallet af tildelte medarbejdere
            var update = Builders<Events>.Update.Push("TaskList.$.AssignmentList", newAssignment)
                .Inc("TaskList.$.EmployeesAssigned", 1);
            await collection.UpdateOneAsync(filter, update); // Udfører opdateringen
        }

        // Sletter et event baseret på eventID
        public async Task DeleteEvent(int eventId)
        {
            await collection.DeleteOneAsync(Builders<Events>.Filter.Eq(r => r.Id, eventId)); // Sletter eventet, hvor id matcher eventId
        }
    }
}
