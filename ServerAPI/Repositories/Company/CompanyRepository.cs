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
            var password = ""; // Tilføj din adgangskode her
            var mongoUri = $"mongodb+srv://Database:ggST93XBrlthKDcp@kantinesystem.ex4dr.mongodb.net/";
            
            // Opretter en forbindelse til MongoDB
            client = new MongoClient(mongoUri);
          
            // Database og samling navn
            var dbName = "KantineDatabase";
            var collectionName = "Companies";

            // Opretter forbindelse til den specifikke samling
            collection = client.GetDatabase(dbName)
                .GetCollection<Company>(collectionName);
        }

        public void AddCompany(Company newCompany)
        {
            try
            {
                // Find det maksimale ID for at sikre, at vi giver et unikt ID til den nye virksomhed
                var max = 0;
                if (collection.CountDocuments(Builders<Company>.Filter.Empty) > 0)
                {
                    max = collection.Find(Builders<Company>.Filter.Empty)
                        .SortByDescending(c => c.Id)
                        .Limit(1)
                        .FirstOrDefault()
                        ?.Id ?? 0;
                }
                newCompany.Id = max + 1;

                // Indsætter den nye virksomhed i databasen
                collection.InsertOne(newCompany);
            }
            catch (Exception ex)
            {
                // Logger eventuelle fejl, der opstår under indsættelsen
                Console.WriteLine($"Fejl opstod under tilføjelsen af virksomheden: {ex.Message}");
                throw; // Kaster undtagelsen videre, så controlleren kan håndtere den
            }
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            // Returnerer alle virksomheder fra databasen
            return collection.Find(Builders<Company>.Filter.Empty).ToList();
        }
    }
}
