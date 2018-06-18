using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiarsDice
{
    public class Game
    {
        public List<Player> Players;
        public Player Winner;
        public Player Loser;
        public GameOptions gameOptions;


        public void AddPlayer() => Players.Add(new Player());

        public void RemovePlayer(int index) => Players.RemoveAt(index);

        public void Start()
        {
            Players.ForEach(player => player.ApplyOptions(gameOptions));
        }
    }
}
