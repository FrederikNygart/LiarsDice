using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnydService;
using SnydService.DataTransferObjects;
using SnydService.DataProviders;

namespace Debugger
{
    class Program
    {
        public static GameEngine game = new GameEngine(); 
        static void Main(string[] args)
        {
            var list1 = new List<int> { 1, 2 };
            var list2 = new List<int> { 1, 2 };
            Console.Write($"YOOOO it is {list1.Equals(list2)}");
            Console.ReadKey();
        }

        private static void TestStartGame()
        {
            var users = new List<ObjectId> {
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId()
            };

            var gameOptions = new GameOptions();
            gameOptions.AmountOfDice = 4;
            gameOptions.AmountOfLives = 3;
            gameOptions.Stair = true;

            game.Start(users, gameOptions);
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
