using LiarsDice;
using LiarsDice.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LiarsDice.DatabaseConfig.LiarsContext;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void TestInsert()
        {
            var player2 = new PlayerData();

            player2.Lives = 2;


            var player = new PlayerData();

            player.Lives = 2;

            InsertPlayer(player).Wait();
            InsertPlayer(player2).Wait();

            Console.Write("Inserted plaeyrs");
            Console.Read();
        }

        static async Task InsertPlayer(PlayerData player)
        {
            await InsertPlayerAsync(player);
        }
    }
}
