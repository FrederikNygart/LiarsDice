using UserService.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Resources.Strings;

namespace UserService.DatabaseContexts
{
    public static class LiarsContext
    {
        public static IMongoDatabase db
        {
            get
            {
                MongoClient client = new MongoClient(Connection);
                return client.GetDatabase(LiarsDiceDataBaseName);
            }
        }

        public static IMongoCollection<Player> Players
        {
            get
            {
                return db.GetCollection<Player>(LiarsDiceContextPlayers);
            }
        }

        public static IMongoCollection<Player> Games
        {
            get
            {
                return db.GetCollection<Player>(LiarsDiceContextGames);
            }
        }
    }
}
