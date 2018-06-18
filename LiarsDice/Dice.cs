using System;

namespace LiarsDice
{
    public class Dice
    {
        public int Eyes;

        public Dice()
        {
            Roll();
        }

        internal void Roll()
        {
            var rnd = new Random();
            Eyes = rnd.Next(1, 7);
        }
    }
}