using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiarsDiceService.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using static LiarsDiceService.DatabaseContexts.LiarsContext;

namespace LiarsDiceService.DataProviders
{
    class GameProvider
    {
        #region CREATE

        public async static Task InsertGameAsync(Game game) 
            => await Games.InsertOneAsync(game);

        internal static List<Player> GetPlayers(ObjectId id) 
            => Get(id).Players.Select(playerId => PlayerProvider.Get(playerId)).ToList();

        #endregion



        #region READ

        public static Game Get(ObjectId id) 
            => Games.Find(p => p.Id == id).Single();

        internal static Player GetLiar(ObjectId id) 
            => PlayerProvider.Get(Get(id).Liar);

        #endregion



        #region UPDATE


        public static UpdateResult UpdateGame<T>(
            ObjectId playerId,
            System.Linq.Expressions.Expression<Func<Game, T>> currentValueExpression,
            T updateValue)
        {
            var updateDefinition = Builders<Game>.Update.Set(currentValueExpression, updateValue);
            return Games.UpdateOne<Game>(p => p.Id == playerId, updateDefinition);
        }

        

        #endregion



        #region DELETE

        #endregion
    }
}
