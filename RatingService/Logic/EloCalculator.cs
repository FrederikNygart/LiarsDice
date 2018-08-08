using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingService.Logic

{
    public static class EloCalculator
    {
        public static long CalculateWin(int amountOfGames, long currentElo) => (currentElo + 400) / amountOfGames;
        public static long CalculateLoss(int amountOfGames, long currentElo) => (currentElo - 400) / amountOfGames;
    }
}
