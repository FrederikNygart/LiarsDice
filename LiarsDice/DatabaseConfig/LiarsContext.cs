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

        public static IMongoCollection<Player> Players
        {
            get
            {
                return db.GetCollection<Player>("Players");
            }
        }

        public static IMongoCollection<Player> Games
        {
            get
            {
                return db.GetCollection<Player>("Games");
            }
        }


        public async static Task InsertPlayerAsync(Player player)
        {
            var collection = db.GetCollection<Player>("Players");
            await collection.InsertOneAsync(player);
        }


    }
}
