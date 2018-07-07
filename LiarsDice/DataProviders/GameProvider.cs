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

        #endregion


        #region READ

        public static List<ObjectId> GetAllPlayer(ObjectId gameId) 
            => Games.Find(g => g.Id == gameId).Single().Players;

        public static Game Get(ObjectId id)
            => Games.Find(p => p.Id == id).Single();

        internal static Player GetLiar(ObjectId gameId)
            => PlayerProvider.Get(Get(gameId).Liar);

        internal static List<Bid> GetBids(ObjectId gameId)
            => Get(gameId).Bids;

        internal static List<Player> GetPlayers(ObjectId gameId)
            => Get(gameId).Players.Select(playerId => PlayerProvider.Get(playerId)).ToList();

        internal static Player GetPreviousPlayer(ObjectId gameId)
            => PlayerProvider.Get(Get(gameId).PreviousPlayer);

        internal static Player GetCurrentPlayer(ObjectId gameId)
            => PlayerProvider.Get(Get(gameId).CurrentPlayer);

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

        internal static void SetPlayers(ObjectId id, List<Player> players) 
            => UpdateGame(id, g => g.Players, players.Select(p => p.Id));

        internal static void SetGameOptions(ObjectId gameId, GameOptions gameOptions)
            => UpdateGame(gameId, g => g.GameOptions, gameOptions);

        public static void SetCurrentPlayer(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.CurrentPlayer, player.Id);

        internal static void SetBids(ObjectId gameId, List<Bid> bids)
            => UpdateGame(gameId, g => g.Bids, bids);

        internal static void SetPreviousPlayer(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.PreviousPlayer, player.Id);

        internal static void SetLiar(ObjectId gameId, Player player)
            => UpdateGame(gameId, g => g.Liar, player.Id);


        #endregion


        #region DELETE

        #endregion
    }
}
