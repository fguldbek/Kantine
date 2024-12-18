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
            // MongoDB URI for forbindelse til MongoDB-databasen
            var mongoUri = "mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";

            // Opretter forbindelse til MongoDB
            client = new MongoClient(mongoUri);

            // Database og collection navn, som vi vil arbejde med
            var dbName = "KantineDatabase"; 
            var collectionName = "Communication"; 

            // Opretter forbindelse til den specifikke samling "Communication"
            collection = client.GetDatabase(dbName)
                .GetCollection<Communication>(collectionName);
        }
        
        // Metode til at sende en ny besked
        public void SendMessage(Communication newMessage)
        {
            var max = 0; // Variabel til at holde styr på den højeste ID-værdi

            // Tjekker om der findes nogen dokumenter i samlingen
            if (collection.CountDocuments(Builders<Communication>.Filter.Empty) > 0)
            {
                // Finder det nyeste (sidste) besked-id og sætter max til dette id
                max = collection.Find(Builders<Communication>.Filter.Empty)
                    .SortByDescending(message => message.Id) // Sorterer efter ID i faldende rækkefølge
                    .Limit(1) // Begrænser til kun ét resultat
                    .First() // Henter det første resultat (nyeste besked)
                    .Id;
            }
            
            // Sætter ID'et på den nye besked
            newMessage.Id = max + 1;

            // Indsætter den nye besked i MongoDB
            collection.InsertOne(newMessage);
        }
        
        // Metode til at hente alle beskeder for en specifik modtager baseret på UserId
        public Communication[] GetAllMessageWithID(int UserId)
        {
            // Filtrerer dokumenterne for at finde beskeder hvor userid er det samme som IdOfReceiver
            var filter = Builders<Communication>.Filter.Eq("IdOfReceiver", UserId);
            
            // Henter alle beskeder, der matcher filteret og konverterer resultatet til et array
            return collection.Find(filter).ToList().ToArray();
        }
    }
}
