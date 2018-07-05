using LiarsDiceService.DataProviders;
using LiarsDiceService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LiarsDiceService
{
    public class GameEngine
    {
        private Game game;
        private GameOptions gameOptions => GameProvider.Get(game.Id).gameOptions;
        private Player liar => GameProvider.GetLiar(game.Id);
        private List<Player> players => GameProvider.GetPlayers(game.Id);

        public bool IsGameOver => liar.Lives <= 0;

        public GameEngine()
        {
            game = new Game();
        }

        public void Start(List<ObjectId> users, GameOptions gameOptions)
        {
            var players = users.Select(user => new Player
                {
                    User = user,
                    Game = game.Id,
                    Dice = new int[gameOptions.AmountOfDice],
                    Lives = gameOptions.AmountOfLives
                }).ToList();

            PlayerProvider.InsertPlayersAsync(players);
            GameProvider.

            SetCurrentPlayer(this.players[0]);
        }

        public void AddPlayer() => players.Add(new Player());

        public void RemovePlayer(int index) => players.RemoveAt(index);

        public void Bid(int quantity, int faceValue) => Bids.Add(new Bid { Quantity = quantity, FaceValue = faceValue });

        public void RotatePlayers()
        {
            SetPreviousPlayer(CurrentPlayer);
            SetCurrentPlayer(GetNextPlayer());
        }


        public void Challenge()
        {
            var lastBid = GetLastBid();
            var totalQuantityOfFaceValue = GetQuantityOfFaceValue(players, lastBid.FaceValue);

            var isBidTrue = totalQuantityOfFaceValue >= lastBid.Quantity;

            liar = isBidTrue ? CurrentPlayer : PreviousPlayer;
        }

        private int GetQuantityOfFaceValue(List<Player> players, int faceValue)
            => players.Aggregate(0, (sum, player) => sum + player.Dice.Where(die => die == faceValue).Count());

        public void SpotOn()
        {
            var lastBid = GetLastBid();
            liar = GetQuantityOfFaceValue(players, lastBid.FaceValue) == lastBid.Quantity ? CurrentPlayer : PreviousPlayer;
        }

        public void PenaliseLiar()
        {
            var winningPlayers = players.Where(player => !player.Equals(liar)).Select(player => RemoveDice(player));
            if (winningPlayers.All(player => player.Dice.Count() == 0)) RemoveLive(liar);
        }

        private void RemoveLive(Player liar)
        {
            PlayerProvider.UpdateLives(liar.Id, liar.Lives - 1);
        }

        private Player RemoveDice(Player player)
        {
            player.Dice = new int[player.Dice.Count() - 1];
            return player;
        }

        public void RollDice() => players.ForEach(player => RollDice(player));

        private void RollDice(Player player)
        {
            player.Dice = player.Dice.Select(die => new Random().Next(1, 6)).ToArray();
            PlayerProvider.UpdateDiceValue(player.Id, player.Dice);
        }

        private Bid GetLastBid() => Bids.Last();

        private void SetPreviousPlayer(Player player) => PreviousPlayer = player;

        private void SetCurrentPlayer(Player player) => CurrentPlayer = player;

        private Player GetNextPlayer()
        {
            var nextPlayerIndex = players.IndexOf(CurrentPlayer) + 1;
            if (nextPlayerIndex < players.Count) return players[nextPlayerIndex];
            return players[0];
        }

        private void ApplyGameOptions(GameOptions gameOptions) => players.ForEach(player => ApplyOptions(gameOptions, player));

        private void ApplyOptions(GameOptions gameOptions, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
