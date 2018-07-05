using LiarsDiceService;
using LiarsDiceService.DataProviders;
using LiarsDiceService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugger
{
    class Program
    {
        public static GameEngine game = new GameEngine(); 
        static void Main(string[] args)
        {
            TestGame();
        }

        private static void TestGame()
        {
            throw new NotImplementedException();
        }

        static void TestInsert()
        {
            var player = new Player();
            var player2 = new Player();
            
            player2.Lives = 2;
            player.Lives = 2;

            PlayerProvider.InsertPlayerAsync(player).Wait();
            PlayerProvider.InsertPlayerAsync(player2).Wait();

            Console.Write("Inserted plaeyrs");
            Console.Read();
        }
    }
}
