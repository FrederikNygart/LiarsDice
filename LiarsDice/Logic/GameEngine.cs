using UserService.DataProviders;
using UserService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace UserService
{
    public class GameEngine
    {
        private Game game;

        public bool IsGameOver => Liar.Lives <= 0;

        public GameEngine()
        {
            game = new Game();
        }

        public void Start(List<ObjectId> users, GameOptions gameOptions)
        {
            this.gameOptions = gameOptions;

            Players = users.Select(user => new Player
                {
                    User = user,
                    Game = game.Id,
                    Dice = new int[gameOptions.AmountOfDice],
                    Lives = gameOptions.AmountOfLives
                }).ToList();  



            SetCurrentPlayer(Players[0]);
        }

        public void AddPlayer() => Players.Add(new Player());

        public void RemovePlayer(int index) => Players.RemoveAt(index);

        public void Bid(int quantity, int faceValue) => Bids.Add(new Bid { Quantity = quantity, FaceValue = faceValue });

        public void RotatePlayers()
        {
            SetPreviousPlayer(CurrentPlayer);
            SetCurrentPlayer(GetNextPlayer());
        }


        public void Challenge()
        {
            var lastBid = GetLastBid();
            var totalQuantityOfFaceValue = GetQuantityOfFaceValue(Players, lastBid.FaceValue);

            var isBidTrue = totalQuantityOfFaceValue >= lastBid.Quantity;

            Liar = isBidTrue ? CurrentPlayer : PreviousPlayer;
        }

        private int GetQuantityOfFaceValue(List<Player> players, int faceValue)
            => players.Aggregate(0, (sum, player) => sum + player.Dice.Where(die => die == faceValue).Count());

        public void SpotOn()
        {
            var lastBid = GetLastBid();
            Liar = GetQuantityOfFaceValue(Players, lastBid.FaceValue) == lastBid.Quantity ? CurrentPlayer : PreviousPlayer;
        }

        public void PenaliseLiar()
        {
            var winningPlayers = Players.Where(player => !player.Equals(Liar)).Select(player => RemoveDice(player));
            if (winningPlayers.All(player => player.Dice.Count() == 0)) RemoveLive(Liar);
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

        public void RollDice() => Players.ForEach(player => RollDice(player));

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
            var nextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;
            if (nextPlayerIndex < Players.Count) return Players[nextPlayerIndex];
            return Players[0];
        }

        private void ApplyGameOptions(GameOptions gameOptions) => Players.ForEach(player => ApplyOptions(gameOptions, player));

        private void ApplyOptions(GameOptions gameOptions, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
