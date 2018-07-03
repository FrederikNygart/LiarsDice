using LiarsDice.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiarsDice
{
    public class Player
    {
        internal void RemoveDice()
        {
            if (CupNotEmpty) Dice = new int[Dice.Count() - 1];
        }

        internal void ApplyOptions(GameOptions gameOptions)
        {
            SetAmountOfDice(gameOptions.AmountOfDice);
        }

        private void SetAmountOfDice(int amountOfDice) => Dice = new int[amountOfDice];

        private static int Roll()
        {
            return DiceThrow.Next(1, 7);
        }


    }
}