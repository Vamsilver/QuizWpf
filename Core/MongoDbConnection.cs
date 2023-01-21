using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWpf.Core
{
    class MongoDbConnection
    {
        public static IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }

        public static void AddToDatabse(Question question)
        {
            GetCollection<Question>("Quiz", "QuestionsCollection").InsertOne(question);
        }

        public static Question Find(int index)
        {
            return GetCollection<Question>("Quiz", "QuestionsCollection").AsQueryable().ToList().ElementAt(index);
        }

        public static int GetQuantity()
        {
            return GetCollection<Question>("Quiz", "QuestionsCollection").AsQueryable().Count();
        }

        public static Question FindRandom()
        {
            var collection = GetCollection<Question>("Quiz", "QuestionsCollection");

            var randomSkip = new Random().Next(0, GetQuantity());

            var result = collection
                        .AsQueryable()
                        .Where(x => x.Quaere != null)
                        .Skip(randomSkip)
                        .FirstOrDefault();

            return result;
        }
    }
}
