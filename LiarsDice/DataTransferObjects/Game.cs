using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DataTransferObjects
{
    public class Game
    {
        public ObjectId Id { get; set; }
        public ObjectId GameOptions { get; set; }
        public List<ObjectId> Players { get; set; }
        public ObjectId CurrentPlayer;
        public ObjectId PreviousPlayer;
        private ObjectId Liar;
        public GameOptions gameOptions;
        public List<Bid> Bids;
        public Bid LastBid;
    }
}
