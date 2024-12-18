using System;
using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{

    public class CompanyRepository : ICompanyRepository
    {
        private IMongoClient client;
        private IMongoCollection<Company> collection;

        public CompanyRepository()
        {
            // MongoDB URI, som indeholder forbindelsesstrengen til database
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";
            
            // Opretter en forbindelse til MongoDB
            client = new MongoClient(mongoUri);
          
            // Database og samling navn, som vi vil arbejde med
            var dbName = "KantineDatabase";
            var collectionName = "Companies";

            // Opretter forbindelse til den specifikke samling "Companies"
            collection = client.GetDatabase(dbName)
                .GetCollection<Company>(collectionName);
        }

        // Metode til at tilføje en ny virksomhed til databasen
        public void AddCompany(Company newCompany)
        {
                var max = 0; // Variabel til at holde styr på den højeste ID-værdi

                // Tjekker om der findes nogen dokumenter i samlingen
                if (collection.CountDocuments(Builders<Company>.Filter.Empty) > 0)
                {
                    // Finder det nyeste (sidste) besked-id og sætter max til dette id
                    max = collection.Find(Builders<Company>.Filter.Empty)
                        .SortByDescending(company => company.Id) // Sorterer efter ID i faldende rækkefølge
                        .Limit(1) // Begrænser resultatet til 1 (den nyeste virksomhed)
                        .FirstOrDefault() // Henter den første virksomhed
                        ?.Id ?? 0; // Hvis ingen virksomheder findes, sætter vi max til 0
                }
                
                // Tildeler den næste ID-værdi til den nye virksomhed
                newCompany.Id = max + 1;

                // Indsætter den nye virksomhed i MongoDB
                collection.InsertOne(newCompany);
        }

        // Metode til at hente alle virksomheder fra databasen
        public IEnumerable<Company> GetAllCompanies()
        {
            // Returnerer alle virksomheder fra samlingen som en liste
            return collection.Find(Builders<Company>.Filter.Empty).ToList();
        }
    }
}
