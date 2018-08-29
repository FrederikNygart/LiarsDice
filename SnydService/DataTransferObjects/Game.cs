using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnydService.DataTransferObjects
{
    public class Game
    {
        public ObjectId Id { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public Player PreviousPlayer { get; set; }
        public Player Liar { get; set; }
        public GameOptions GameOptions { get; set; }
        public List<Bid> Bids { get; set; } = new List<Bid>();
        public Bid LastBid { get; set; }
    }
}
