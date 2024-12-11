using System;
using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
    public class CommunicationRepository : ICommunicationRepository
    {
        private IMongoClient client;
        private IMongoCollection<Communication> collection;

        public CommunicationRepository()
        {
            var password = ""; // Tilføj din adgangskode her
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            try
            {
                // Opretter en forbindelse til MongoDB
                client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                Console.WriteLine("Der opstod et problem med at oprette forbindelse til din Atlas-kluster. Tjek, at URI'en indeholder et gyldigt brugernavn og adgangskode, og at din IP-adresse er i adgangspunktlisten.");
                Console.WriteLine(e.Message);
                return;
            }

            // Database og samling navn
            var dbName = "KantineDatabase";
            var collectionName = "Communication";

            // Opretter forbindelse til den specifikke samling
            collection = client.GetDatabase(dbName)
                .GetCollection<Communication>(collectionName);
        }
        
        public void SendMessage(Communication newMessage)
        {
            var max = 0;
            if (collection.CountDocuments(Builders<Communication>.Filter.Empty) > 0)
            {
                max = collection.Find(Builders<Communication>.Filter.Empty)
                    .SortByDescending(r => r.Id)
                    .Limit(1)
                    .First()
                    .Id;
            }
            newMessage.Id = max + 1;
            collection.InsertOne(newMessage);
        }
        
        public Communication[] GetAllMessageWithID(int UserId)
        {
                // Using MongoDB dot notation to access the nested 'User.BuyerId' field in the filter
                var filter = Builders<Communication>.Filter.Eq("IdOfReceiver", UserId);
                return collection.Find(filter).ToList().ToArray();
        }
    }
}
