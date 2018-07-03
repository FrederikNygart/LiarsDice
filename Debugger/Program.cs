using LiarsDice;
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
            var player2 = new Player();

            player2.Lives = 2;
            player2.name = "Jesper";


            var player = new Player();

            player.Lives = 2;
            player.name = "Marianne";

            InsertPlayer(player).Wait();
            InsertPlayer(player2).Wait();

            Console.Write("Inserted plaeyrs");
            Console.Read();
        }

        static async Task InsertPlayer(Player player)
        {
            await InsertPlayerAsync(player);
        }
    }
}
