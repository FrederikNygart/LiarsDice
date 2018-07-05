using UserService.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static UserService.DatabaseContexts.LiarsContext;

namespace UserService.DataProviders
{
    public static class PlayerProvider
    {

        #region CREATE

        public async static Task InsertPlayerAsync(Player player) => await Players.InsertOneAsync(player);

        #endregion

        #region READ

        public static Player Get(ObjectId id) => Players.Find(p => p.Id == id).Single();

        #endregion

        #region UPDATE

        public static void UpdateLives(ObjectId playerId, int amountOfLives) => UpdatePlayer(playerId, p => p.Lives, amountOfLives);

        public static void UpdateDiceValue(ObjectId playerId, int[] dice) => UpdatePlayer(playerId, p => p.Dice, dice);

        public static UpdateResult UpdatePlayer<T>(
            ObjectId playerId,
            System.Linq.Expressions.Expression<Func<Player, T>> currentValueExpression,
            T updateValue)
        {
            var updateDefinition = Builders<Player>.Update.Set(currentValueExpression, updateValue);
            return Players.UpdateOne<Player>(p => p.Id == playerId, updateDefinition);
        }

        #endregion

        #region DELETE
        #endregion






    }
}
