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
        private Game game;
        private GameOptions GameOptions => GameProvider.Get(game.Id).GameOptions;
        private Player Liar => GameProvider.GetLiar(game.Id);
        private Player PreviousPlayer => GameProvider.GetPreviousPlayer(game.Id);
        private Player CurrentPlayer => GameProvider.GetLiar(game.Id);
        private List<Player> Players => GameProvider.GetPlayers(game.Id);
        private List<Bid> Bids => GameProvider.GetBids(game.Id);

        public bool IsGameOver => Liar.Lives <= 0;

        public GameEngine()
        {
            game = new Game();
            GameProvider.InsertGameAsync(game).Wait();
        }

        public void SetGameOptions()
        {
            SetGameOptions(gameOptions);
        }

        public void Start(List<ObjectId> users)
        {
            var players = users.Select(user => new Player
            {
                User = user,
                Game = game.Id,
                Dice = new int[gameOptions.AmountOfDice],
                Lives = gameOptions.AmountOfLives
            }).ToList();
            PlayerProvider.InsertPlayersAsync(players).Wait();
            SetPlayers(players);
            SetCurrentPlayer(this.Players[0]);
        }

        private void SetGameOptions(GameOptions gameOptions)
            => GameProvider.SetGameOptions(game.Id, gameOptions);

        private void SetPlayers(List<Player> players)
            => GameProvider.SetPlayers(game.Id, players);

        public void AddPlayer()
            => Players.Add(new Player());

        public void RemovePlayer(int index)
            => Players.RemoveAt(index);

        public void Bid(int quantity, int faceValue)
        {
            Bids.Add(new Bid { Quantity = quantity, FaceValue = faceValue });
            SetBids(Bids);
        }

        public void BidOfAKind(int quantity)
        {
            Bids.Add(new Bid { Quantity = quantity, FaceValue = 0 });
            SetBids(Bids);
        }

        private void SetBids(List<Bid> bids)
            => GameProvider.SetBids(game.Id, bids);

        public void RotatePlayers()
        {
            SetPreviousPlayer(CurrentPlayer);
            SetCurrentPlayer(GetNextPlayer());
        }

        public void Challenge()
        {
            var lastBid = GetLastBid();
            Player liar;
            if(GameOptions.OfAKind && lastBid.FaceValue == 0)
            {
               var amountOfAKind = Enumerable.Range(1, 6)
                    .Select(faceVal => GetEvaluationMethod(faceVal))
                    .Select(evaluation => GetQuantityOfFaceValue(Players, evaluation))
                    .Max();

                var isBidTrue = amountOfAKind >= lastBid.Quantity;

                liar = isBidTrue 
                     ? CurrentPlayer
                     : PreviousPlayer;

            }
            else
            {
                var isBidTrue = 
                    GetQuantityOfFaceValue(Players, GetEvaluationMethod(lastBid.FaceValue)) 
                    >= lastBid.Quantity;

                liar = isBidTrue 
                    ? CurrentPlayer 
                    : PreviousPlayer;
            }
            SetLiar(liar);
        }

        private Func<int, bool> GetEvaluationMethod(int faceValue)
            => GameOptions.LuckyOnes ? LuckyOnesEvaluation(faceValue) : NormalEvaluation(faceValue);

        private Func<int, bool> NormalEvaluation(int faceValue)
            => die => die == faceValue;

        private Func<int, bool> LuckyOnesEvaluation(int faceValue)
            => die => die == faceValue || die == 1;

        private int GetQuantityOfFaceValue(List<Player> players, Func<int, bool> predicate)
        {
            var quantity = 0;
            foreach (var player in players)
            {
                if (GameOptions.Stair && HasStair(player)) quantity += player.Dice.Count() + 1;

                else quantity += player.Dice.Where(predicate).Count();
            }
            return quantity;
        }

        private bool HasStair(Player player) 
            => Enumerable.Range(1, player.Dice.Count()).All(die => player.Dice.Contains(die));

        public void SpotOn()
        {
            var lastBid = GetLastBid();
            var liar = GetQuantityOfFaceValue(Players, GetEvaluationMethod(lastBid.FaceValue)) == lastBid.Quantity ? CurrentPlayer : PreviousPlayer;
            SetLiar(liar);
        }

        public void PenaliseLiar()
        {
            var winningPlayers = Players.Where(player => !player.Equals(Liar));

            RemoveDice(winningPlayers);

            if (winningPlayers.All(player => player.Dice.Count() == 0)) RemoveLive(Liar);
        }

        private void RemoveDice(IEnumerable<Player> winningPlayers)
        {
            foreach (var player in winningPlayers)
            {
                RemoveDice(player);
            }
        }

        private void RemoveLive(Player liar)
            => PlayerProvider.SetLives(liar.Id, liar.Lives - 1);

        private void RemoveDice(Player player)
            => PlayerProvider.SetDice(player.Id, new int[player.Dice.Count() - 1]);

        public void RollDice()
            => Players.ForEach(player => RollDice(player));

        private void RollDice(Player player)
        {
            player.Dice = player.Dice.Select(die => new Random().Next(1, 6)).ToArray();
            PlayerProvider.SetDice(player.Id, player.Dice);
        }

        private Bid GetLastBid()
            => Bids.Last();

        private void SetPreviousPlayer(Player player)
            => GameProvider.SetPreviousPlayer(game.Id, player);

        private void SetCurrentPlayer(Player player)
            => GameProvider.SetCurrentPlayer(game.Id, player);

        private void SetLiar(Player player)
            => GameProvider.SetLiar(game.Id, Liar);

        private Player GetNextPlayer()
        {
            var nextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;
            if (nextPlayerIndex < Players.Count) return Players[nextPlayerIndex];
            return Players[0];
        }
    }
}
