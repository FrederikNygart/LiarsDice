using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnydService.DataTransferObjects
{
    public class Player
    {
        public ObjectId User { get; set; }
        public List<Bid> Bids { get; set; }
        public int Lives { get; set; }
        public int[] Dice { get; set; }
    }
}
