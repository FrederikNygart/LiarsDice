using System;
using System.Collections.Generic;
using System.Linq;

namespace LiarsDice
{
    public class Player
    {
        public int Lives;
        public int[] Dice;
        private bool CupNotEmpty => Dice.Count() > 0;
        private Random DiceThrow = new Random();


        /** GAME INTERFACE public**/
        public Tuple<int, int> Bid()
        {
            throw new NotImplementedException();
        }

        /**GAME ENGINE internals**/
        internal int[] RollDice()
        {
            if (CupNotEmpty)
            {
                Dice = Dice.Select(die => die = Roll()).ToArray();
                return Dice;
            }
            return new int[0];
        }

        internal void RemoveDice()
        {
            if (CupNotEmpty) Dice = new int[Dice.Count() - 1];
        }

        internal void ApplyOptions(GameOptions gameOptions)
        {
            SetAmountOfDice(gameOptions.AmountOfDice);
        }

        private void SetAmountOfDice(int amountOfDice) => Dice = new int[amountOfDice];

        private int Roll()
        {
            return DiceThrow.Next(1, 7);
        }


    }
}