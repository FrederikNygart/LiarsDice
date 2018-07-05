using LiarsDice.DataProviders;
using LiarsDice.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiarsDice
{
    public class GameEngine
    {
        public List<Player> Players;
        public Player CurrentPlayer;
        public Player PreviousPlayer;
        private Player Liar;
        public GameOptions gameOptions;
        public List<Bid> Bids;
        public Bid LastBid;

        public void AddPlayer() => Players.Add(new Player());

        public void RemovePlayer(int index) => Players.RemoveAt(index);

        public void Start()
        {
            ApplyGameOptions(gameOptions);
            SetCurrentPlayer(Players[0]);
            Bids = new List<Bid>();
        }

        public void RotatePlayers()
        {
            SetPreviousPlayer(CurrentPlayer);
            SetCurrentPlayer(GetNextPlayer());
        }

        public void Bid(int amountOfDice, int eyes)
        {
            Bids.Add(new Bid { Quantity = amountOfDice, FaceValue = eyes });
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

        private bool isGameOver() => Liar.Lives <= 0;

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
