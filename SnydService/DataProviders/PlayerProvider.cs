using SnydService.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SnydService.DatabaseContexts.LiarsContext;

namespace SnydService.DataProviders
{
    public static class PlayerProvider
    {

        #region CREATE

        public async static Task InsertPlayerAsync(Player player) 
            => await Players.InsertOneAsync(player);

        public async static Task InsertPlayersAsync(List<Player> players)
            => await Players.InsertManyAsync(players);

        #endregion

        #region READ

        public static Player Get(ObjectId id) => Players.Find(p => p.Id == id).Single();

        #endregion

        #region UPDATE

        public static void SetLives(ObjectId playerId, int amountOfLives) 
            => UpdatePlayer(playerId, p => p.Lives, amountOfLives);

        public static void SetDice(ObjectId playerId, int[] dice) 
            => UpdatePlayer(playerId, p => p.Dice, dice);

        public static UpdateResult UpdatePlayer<T>(
            ObjectId playerId,
            System.Linq.Expressions.Expression<Func<Player, T>> currentValueExpression,
            T updateValue)
        {
            var updateDefinition = Builders<Player>.Update.Set(currentValueExpression, updateValue);
            return Players.UpdateOne(p => p.Id == playerId, updateDefinition);
        }
        
        #endregion

        #region DELETE
        #endregion






    }
}
