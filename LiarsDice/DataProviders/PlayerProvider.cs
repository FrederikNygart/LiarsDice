using LiarsDice.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LiarsDice.DatabaseConfig.LiarsContext;

namespace LiarsDice.DataProviders
{
    public static class PlayerProvider
    {
        public static PlayerData Get(ObjectId id)
        {
            return Players.Find(p => p.Id == id).Single();
        }

        public static void RemoveLive(ObjectId id)
        {
            var player = Get(id);
            var update = Builders<PlayerData>.Update.Set(p => p.Lives, player.Lives -1);
            Players.UpdateOne<PlayerData>(p => p.Id == id, update);
        }
    }
}
