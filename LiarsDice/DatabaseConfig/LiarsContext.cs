using LiarsDice.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiarsDice.DatabaseConfig
{
    public static class LiarsContext
    {
        public static IMongoDatabase db
        {
            get
            {
                var conn = "mongodb://localhost:27017";
                MongoClient client = new MongoClient(conn);
                return client.GetDatabase("LiarsDice");
            }
        }

        public static IMongoCollection<PlayerData> Players
        {
            get
            {
                return db.GetCollection<PlayerData>("Players");
            }
        }

        public static IMongoCollection<PlayerData> Games
        {
            get
            {
                return db.GetCollection<PlayerData>("Games");
            }
        }


        public async static Task InsertPlayerAsync(PlayerData player)
        {
            var collection = db.GetCollection<PlayerData>("Players");
            await collection.InsertOneAsync(player);
        }


    }
}
