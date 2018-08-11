using SnydService.DataProviders;
using SnydService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace SnydService
{
    public class GameEngine
    {
        public GameEngine()
        {
        }

        public ObjectId Start(IEnumerable<ObjectId> users, GameOptions gameOptions)
        {
            var g = new Game();
            GameProvider.Insert(g);
            SetGameOptions(g.Id, gameOptions);
            var players = users.Select(user => new Player
            {
                User = user,
                Game = g.Id,
                Dice = new int[gameOptions.AmountOfDice],
                Lives = gameOptions.AmountOfLives
            }).ToList();
            PlayerProvider.InsertPlayers(players);
            SetPlayers(g.Id,players);
            SetCurrentPlayer(g.Id, players[0]);
            return g.Id;
        }

        public void Bid(ObjectId gameId, Bid bid)
        {
            ValidateBid(gameId, bid);
            var bids = GetBids(gameId);
            bids.Add(bid);
            SetBids(gameId, bids);
            RotatePlayers(gameId);
        }

        public void BidOfAKind(ObjectId gameId, Bid bid)
        {
            if (bid.FaceValue != 0) throw new Exception($"Not possible to accept bid of a kind with a facevalue of: {bid.FaceValue}");
            validateQuantity(gameId, bid);

            var bids = GetBids(gameId);
            bids.Add(bid);
            SetBids(gameId, bids);
            RotatePlayers(gameId);
        }

        public void SpotOn(ObjectId gameId)
        {
            var lastBid = GetLastBid(gameId);
            var liar = GetQuantityOfFaceValue(gameId, GetPlayers(gameId), GetEvaluationMethod(gameId, lastBid.FaceValue)) == lastBid.Quantity
                ? GetCurrentPlayer(gameId)
                : GetPreviousPlayer(gameId);
            SetLiar(gameId, liar);
        }

        public void Challenge(ObjectId gameId)
        {
            var lastBid = GetLastBid(gameId);
            Player liar;
            if (GetGameOptions(gameId).OfAKind && lastBid.FaceValue == 0)
            {
                var amountOfAKind = Enumerable.Range(1, 6)
                     .Select(faceVal => GetEvaluationMethod(gameId, faceVal))
                     .Select(evaluation => GetQuantityOfFaceValue(gameId, GetPlayers(gameId), evaluation))
                     .Max();

                var isBidTrue = amountOfAKind >= lastBid.Quantity;

                liar = isBidTrue
                     ? GetCurrentPlayer(gameId)
                     : GetPreviousPlayer(gameId);

            }
            else
            {
                var isBidTrue =
                    GetQuantityOfFaceValue(gameId, GetPlayers(gameId), GetEvaluationMethod(gameId, lastBid.FaceValue))
                    >= lastBid.Quantity;

                liar = isBidTrue
                     ? GetCurrentPlayer(gameId)
                     : GetPreviousPlayer(gameId);
            }
            SetLiar(gameId, liar);
        }

        public void RollDice(ObjectId gameId)
            => GetPlayers(gameId).ForEach(player => RollDice(player));

        private void RotatePlayers(ObjectId gameId)
        {
            SetPreviousPlayer(gameId, GetCurrentPlayer(gameId));
            SetCurrentPlayer(gameId, GetNextPlayer(gameId));
        }

        private Func<int, bool> GetEvaluationMethod(ObjectId gameId, int faceValue)
            => GetGameOptions(gameId).LuckyOnes ? LuckyOnesEvaluation(faceValue) : NormalEvaluation(faceValue);

        private Func<int, bool> NormalEvaluation(int faceValue)
            => die => die == faceValue;

        private Func<int, bool> LuckyOnesEvaluation(int faceValue)
            => die => die == faceValue || die == 1;

        private int GetQuantityOfFaceValue(ObjectId gameId, List<Player> players, Func<int, bool> predicate)
        {
            var quantity = 0;
            foreach (var player in players)
            {
                if (GetGameOptions(gameId).Stair && HasStair(player)) quantity += player.Dice.Count() + 1;

                else quantity += player.Dice.Where(predicate).Count();
            }
            return quantity;
        }

        private bool HasStair(Player player) 
            => Enumerable.Range(1, player.Dice.Count()).All(die => player.Dice.Contains(die));

        private void PenaliseLiar(ObjectId gameId)
        {
            var liar = GetLiar(gameId);
            var winningPlayers = GetPlayers(gameId).Where(player => !player.Equals(liar));

            RemoveDice(winningPlayers);

            if (winningPlayers.All(player => player.Dice.Count() == 0)) RemoveLive(liar);
        }

        private void RemoveDice(IEnumerable<Player> winningPlayers)
        {
            foreach (var player in winningPlayers)
            {
                RemoveDice(player);
            }
        }

        private Player GetNextPlayer(ObjectId gameId)
        {
            var players = GetPlayers(gameId);
            var nextPlayerIndex = players.IndexOf(GetCurrentPlayer(gameId)) + 1;
            if (nextPlayerIndex < players.Count) return players[nextPlayerIndex];
            return players[0];
        }

        private void RollDice(Player player)
        {
            player.Dice = player.Dice.Select(die => new Random().Next(1, 6)).ToArray();
            PlayerProvider.SetDice(player.Id, player.Dice);
        }

        #region Validation

        private void ValidateBid(ObjectId gameId, Bid bid)
        {
            if (bid.FaceValue > 6
                 || bid.FaceValue < 1) throw new Exception($"Invalid facevalue: {bid.FaceValue}");

            validateQuantity(gameId, bid);
        }

        private void validateQuantity(ObjectId gameId, Bid bid)
        {
            if (bid.Quantity < 1 || bid.Quantity > GetGameOptions(gameId).AmountOfDice)
                throw new Exception($"Invalid quantity: {bid.Quantity}");
        }

        #endregion

        #region Provider functions
        private void RemoveLive(Player liar)
            => PlayerProvider.SetLives(liar.Id, liar.Lives - 1);

        private void RemoveDice(Player player)
            => PlayerProvider.SetDice(player.Id, new int[player.Dice.Count() - 1]);

        private Player GetPlayer(ObjectId playerId) => PlayerProvider.Get(playerId);

        private Game GetGame(ObjectId id) => GameProvider.Get(id);

        private Bid GetLastBid(ObjectId gameId)
            => GetBids(gameId).Last();

        private void SetPreviousPlayer(ObjectId gameId, Player player)
            => GameProvider.SetPreviousPlayer(gameId, player);

        private void SetCurrentPlayer(ObjectId gameId, Player player)
            => GameProvider.SetCurrentPlayer(gameId, player);

        private void SetLiar(ObjectId gameId, Player player)
            => GameProvider.SetLiar(gameId, player);

        private void SetGameOptions(ObjectId gameId, GameOptions gameOptions)
           => GameProvider.SetGameOptions(gameId, gameOptions);

        private void SetPlayers(ObjectId gameId, List<Player> players)
            => GameProvider.SetPlayers(gameId, players);

        private void AddPlayer(ObjectId gameId)
            => GetPlayers(gameId).Add(new Player());

        private void RemovePlayer(ObjectId gameId, int index)
            => GetPlayers(gameId).RemoveAt(index);

        private void SetBids(ObjectId gameId, List<Bid> bids)
            => GameProvider.SetBids(gameId, bids);

        private Player GetLiar(ObjectId gameId)
            => GameProvider.GetLiar(gameId);

        private List<Bid> GetBids(ObjectId gameId)
            => GetGame(gameId).Bids;

        private GameOptions GetGameOptions(ObjectId gameId)
            => GetGame(gameId).GameOptions;

        private List<ObjectId> GetPlayerIds(ObjectId gameId)
            => GetGame(gameId).Players;

        private ObjectId GetCurrentPlayerId(ObjectId gameId)
            => GetGame(gameId).CurrentPlayer;

        private ObjectId GetPreviousPlayerId(ObjectId gameId)
            => GetGame(gameId).PreviousPlayer;

        private List<Player> GetPlayers(ObjectId gameId)
            => GetPlayerIds(gameId).Select(playerId => GetPlayer(playerId)).ToList();

        private Player GetCurrentPlayer(ObjectId gameId)
           => GetPlayer(GetCurrentPlayerId(gameId));

        private Player GetPreviousPlayer(ObjectId gameId)
           => GetPlayer(GetPreviousPlayerId(gameId));
        #endregion
    }
}
