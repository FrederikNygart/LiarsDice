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
        public List<ObjectId> Players { get; set; }
        public ObjectId CurrentPlayer;
        public ObjectId PreviousPlayer;
        public ObjectId Liar;
        public GameOptions GameOptions;
        public List<Bid> Bids;
        public Bid LastBid;
    }
}
