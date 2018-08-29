using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnydService.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using static SnydService.DatabaseContexts.LiarsContext;

namespace SnydService.DataProviders
{
    class GameProvider
    {
        #region CREATE

        public async static Task InsertGameAsync(Game game)
            => await Games.InsertOneAsync(game);

        public static void Insert(Game game) 
            => Games.InsertOne(game);

        #endregion


        #region READ

        public static List<Player> GetAllPlayer(ObjectId gameId) 
            => Games.Find(g => g.Id == gameId).Single().Players;

        public static Game Get(ObjectId id)
            => Games.Find(p => p.Id == id).Single();

        internal static Player GetLiar(ObjectId gameId)
            => Get(gameId).Liar;

        internal static List<Bid> GetBids(ObjectId gameId)
            => Get(gameId).Bids;

        internal static List<Player> GetPlayers(ObjectId gameId)
            => Get(gameId).Players.ToList();

        internal static Player GetPreviousPlayer(ObjectId gameId)
            => Get(gameId).PreviousPlayer;

        internal static Player GetCurrentPlayer(ObjectId gameId)
            => Get(gameId).CurrentPlayer;

        #endregion


        #region UPDATE

        public static UpdateResult UpdateGame<T>(
            ObjectId gameId,
            System.Linq.Expressions.Expression<Func<Game, T>> currentValueExpression,
            T updateValue)
        {
            var updateDefinition = Builders<Game>.Update.Set(currentValueExpression, updateValue);
            return Games.UpdateOne<Game>(game => game.Id == gameId, updateDefinition);
        }

        public static UpdateResult UpdateGameWhere<T>(
            System.Linq.Expressions.Expression<Func<Game, bool>> filter,
            System.Linq.Expressions.Expression<Func<Game, T>> currentValueExpression,
            T updateValue)
        {
            var updateDefinition = Builders<Game>.Update.Set(currentValueExpression, updateValue);
            return Games.UpdateOne<Game>(filter, updateDefinition);
        }

        internal static void SetPlayers(ObjectId id, List<Player> players) 
            => UpdateGame(id, g => g.Players, players);

        internal static UpdateResult SetGameOptions(ObjectId gameId, GameOptions gameOptions)
            => UpdateGame(gameId, g => g.GameOptions, gameOptions);

        public static void SetCurrentPlayer(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.CurrentPlayer, player);

        internal static void SetBids(ObjectId gameId, List<Bid> bids)
            => UpdateGame(gameId, g => g.Bids, bids);

        internal static void SetPreviousPlayer(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.PreviousPlayer, player);

        internal static void SetLiar(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.Liar, player);

        internal static void SetDice(ObjectId gameId, ObjectId userId, int[] dice)
            => UpdateGameWhere(UserIsPlayer(gameId, userId), g => g.Players[-1].Dice, dice);

        internal static void SetLives(ObjectId gameId, ObjectId userId, int lives)
            => UpdateGameWhere(UserIsPlayer(gameId, userId), g => g.Players[-1].Lives, lives);

        private static System.Linq.Expressions.Expression<Func<Game, bool>> UserIsPlayer(ObjectId gameId, ObjectId userId)
            => g => g.Id == gameId && g.Players.Any(p => p.User == userId);

        #endregion


            #region DELETE

            #endregion
    }
}
