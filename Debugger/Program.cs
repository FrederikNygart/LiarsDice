using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnydService;
using SnydService.DataTransferObjects;
using SnydService.DataProviders;
using AuthenticationService.DataProviders;
using AuthenticationService.DataTransferObjects;

namespace Debugger
{
    class Program
    {
        public static GameEngine game = new GameEngine();
        static void Main(string[] args)
        {
            TestCreateUser();
            Console.ReadKey();
        }

        private static void TestCreateUser()
        {
            UserProvider.Insert(
                new User
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Frederik",
                    PhoneNumber = 28269533,
                    EloRating = 1223.23
                });
            Console.WriteLine("UserCreateddd!");
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

            game.Start(users);
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
