using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiarsDice.DataTransferObjects
{
    public class GameData
    {
        public ObjectId Id { get; set; }
        public ObjectId GameOptions { get; set; }
        public List<ObjectId> Players { get; set; }
        
    }
}
