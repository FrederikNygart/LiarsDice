using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiarsDiceService.DataTransferObjects
{
    public class GameOptions
    {
        public int AmountOfDice = 4;
        public bool LuckyOnes = false;
        public bool SpotOn = false;
        public bool OfAKind = false;
        public bool CanBidOnes = false;
        public bool Stair = false;
        public int AmountOfLives = 1;
    }
}
