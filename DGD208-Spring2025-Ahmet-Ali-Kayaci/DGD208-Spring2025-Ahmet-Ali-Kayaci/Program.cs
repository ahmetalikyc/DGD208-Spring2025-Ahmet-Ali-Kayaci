using System;
using System.Threading.Tasks;

namespace PetSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Interactive Pet Simulator!");
            Console.WriteLine("=====================================");

            var game = new Game();
            await game.StartAsync();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}