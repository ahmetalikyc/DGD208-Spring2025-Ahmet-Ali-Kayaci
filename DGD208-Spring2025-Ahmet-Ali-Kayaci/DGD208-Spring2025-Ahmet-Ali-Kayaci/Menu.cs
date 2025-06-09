using System;
using System.Collections.Generic;

namespace PetSimulator
{
    public class Menu
    {
        #region Fields
        private string title;
        private List<string> options;
        #endregion

        #region Constructor
        public Menu(string title, List<string> options)
        {
            this.title = title;
            this.options = options;
        }
        #endregion

        #region Display Methods
        public void Display()
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));
            
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine();
        }

        public static void ShowMainMenuOptions(int currentDay)
        {
            var menuOptions = new List<string>
            {
                "Adopt a Pet", "View Pet Status", "Interact with Pet", 
                "View All Pets", "Display Creator Info", "Exit Game"
            };

            var menu = new Menu("=== MAIN MENU ===", menuOptions);
            Console.WriteLine($"Day {currentDay}");
            menu.Display();
        }

        public static void ShowPetInteractionHeader(Pet pet, int currentDay)
        {
            Console.Clear();
            Console.WriteLine($"=== INTERACTING WITH {pet.Name.ToUpper()} ===");
            Console.WriteLine($"Current Status: {pet.GetCurrentStatus()}");
            Console.WriteLine($"Day {currentDay} - {pet.Name}'s actions today: {pet.GetDailyActionCount()}/3");
            Console.WriteLine();
        }

        public static void ShowActivityOptions(string activityType)
        {
            List<string> options = activityType switch
            {
                "FEED" => new List<string> { "Carrots", "Cat Food", "Dog Food", "Bird Seeds" },
                "PLAY" => new List<string> { "Throw Ball", "Feather Wand Play", "Talk and Sing", "Tunnel Exploration" },
                "REST" => new List<string> { "Put in Dog House", "Warm Cushion Rest", "Cover Cage for Sleep", "Tunnel Hideaway" },
                _ => new List<string>()
            };

            var menu = new Menu($"=== CHOOSE {activityType} OPTION ===", options);
            menu.Display();
        }
        #endregion

        #region Option Management
        public void AddOption(string option)
        {
            options.Add(option);
        }

        public void RemoveOption(string option)
        {
            options.Remove(option);
        }

        public int GetOptionCount()
        {
            return options.Count;
        }

        public string GetOption(int index)
        {
            if (index >= 0 && index < options.Count)
                return options[index];
            return null;
        }
        #endregion
    }
}