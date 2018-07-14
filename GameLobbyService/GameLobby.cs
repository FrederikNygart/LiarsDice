using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyService
{
    public static class GameLobby
    {
        public static List<ObjectId> Queue;

        public static void EnterQueue(ObjectId user) => Queue.Add(user);
        public static void LeaveQueue(ObjectId user) => Queue.Remove(user);
        public static List<ObjectId> GetQueue() => Queue;

    }
}
