using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetSimulator
{
    public class Game
    {
        #region Fields
        private ItemDatabase itemDatabase;
        private PetManager petManager;
        private bool isRunning;
        #endregion

        #region Constructor
        public Game()
        {
            itemDatabase = new ItemDatabase();
            petManager = new PetManager();
            isRunning = true;
            
            EventSystem.OnGameMessage += DisplayMessage;
            EventSystem.OnPetAdopted += HandlePetAdopted;
        }
        #endregion

        #region Main Game Loop
        public async Task StartAsync()
        {
            EventSystem.TriggerGameMessage("Game started! Take care of your pets like real animals!");
            
            while (isRunning)
            {
                await ShowMainMenuAsync();
            }
            
            EventSystem.TriggerGameMessage("Thanks for playing!");
        }

        private async Task ShowMainMenuAsync()
        {
            Menu.ShowMainMenuOptions(petManager.GetCurrentDay());

            Console.Write("Enter your choice (1-6): ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                await ExecuteMainMenuChoice(choice);
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid input! Please enter a number.");
            }
        }

        private async Task ExecuteMainMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1: await AdoptPetAsync(); break;
                case 2: await ViewPetStatusAsync(); break;
                case 3: await InteractWithPetAsync(); break;
                case 4: petManager.DisplayAllPets(); break;
                case 5: DisplayCreatorInfo(); break;
                case 6: isRunning = false; break;
                default: EventSystem.TriggerGameMessage("Invalid choice! Please try again."); break;
            }

            if (isRunning)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        #endregion

        #region Pet Management
        private async Task AdoptPetAsync()
        {
            var petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>().ToList();
            var menu = new Menu("=== ADOPT A PET ===", petTypes.Select(pt => pt.ToString()).ToList());
            menu.Display();

            Console.Write($"Choose pet type (1-{petTypes.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= petTypes.Count)
            {
                Console.Write("Enter pet name: ");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    var pet = new Pet(name, petTypes[choice - 1]);
                    petManager.AddPet(pet);
                    EventSystem.TriggerPetAdopted(pet);
                }
                else
                {
                    EventSystem.TriggerGameMessage("Invalid name! Pet adoption cancelled.");
                }
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid choice! Pet adoption cancelled.");
            }
        }

        private async Task ViewPetStatusAsync()
        {
            if (!petManager.HasAnyPets())
            {
                EventSystem.TriggerGameMessage("You don't have any pets yet!");
                return;
            }

            var alivePets = petManager.GetAlivePets();
            if (!alivePets.Any())
            {
                EventSystem.TriggerGameMessage("All your pets have passed away. Consider adopting new ones!");
                return;
            }

            var petOptions = alivePets.Select(p => $"{p.Name} ({p.Type})").ToList();
            petOptions.Add("View All Pets");

            var menu = new Menu("=== VIEW PET STATUS ===", petOptions);
            menu.Display();

            Console.Write($"Choose pet (1-{petOptions.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == petOptions.Count)
                    petManager.DisplayAllPets();
                else if (choice >= 1 && choice <= alivePets.Count)
                    StatManager.DisplayPetDetails(alivePets[choice - 1], petManager.GetCurrentDay());
                else
                    EventSystem.TriggerGameMessage("Invalid choice!");
            }
        }

        private async Task InteractWithPetAsync()
        {
            if (!petManager.HasAlivePets())
            {
                EventSystem.TriggerGameMessage("You don't have any pets to interact with!");
                return;
            }

            var alivePets = petManager.GetAlivePets();
            var menu = new Menu("=== SELECT PET TO INTERACT WITH ===", alivePets.Select(p => $"{p.Name} ({p.Type})").ToList());
            menu.Display();

            Console.Write($"Choose pet (1-{alivePets.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= alivePets.Count)
            {
                await ShowPetInteractionMenuAsync(alivePets[choice - 1]);
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid choice!");
            }
        }
        #endregion

        #region Pet Interactions
        private async Task ShowPetInteractionMenuAsync(Pet pet)
        {
            while (true)
            {
                if (!pet.IsAlive)
                {
                    Console.Clear();
                    Console.WriteLine($"*** {pet.Name} has passed away. ***");
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    return;
                }

                Menu.ShowPetInteractionHeader(pet, petManager.GetCurrentDay());

                if (!pet.CanDoMoreActions())
                {
                    Console.WriteLine($"*** {pet.Name} has completed 3 actions today! ***");
                    
                    if (CheckAllPetsCompletedActions())
                    {
                        Console.WriteLine("*** All pets have completed their daily actions! ***");
                        await AdvanceToNextDay();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Choose another pet to continue.");
                        Console.WriteLine("Press any key to return to main menu...");
                        Console.ReadKey();
                        return;
                    }
                }

                var menu = new Menu($"What would you like to do with {pet.Name}?", 
                    new List<string> { "Feed Pet", "Play with Pet", "Put Pet to Rest", "Back to Main Menu" });
                menu.Display();

                Console.Write("Choose interaction (1-4): ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1: await FeedPetAsync(pet); break;
                        case 2: await PlayWithPetAsync(pet); break;
                        case 3: await PutPetToRestAsync(pet); break;
                        case 4: return;
                        default: EventSystem.TriggerGameMessage("Invalid choice!"); break;
                    }

                    if (!pet.IsAlive)
                    {
                        Console.WriteLine("Press any key to return to main menu...");
                        Console.ReadKey();
                        return;
                    }

                    if (CheckAllPetsCompletedActions())
                    {
                        return;
                    }

                    Console.WriteLine("\n*** Press any key to continue... ***");
                    Console.ReadKey();
                }
                else
                {
                    EventSystem.TriggerGameMessage("Invalid input! Please enter a number.");
                    Console.ReadKey();
                }
            }
        }

        private bool CheckAllPetsCompletedActions()
        {
            var alivePets = petManager.GetAlivePets();
            
            if (!alivePets.Any())
            {
                return true;
            }
            
            return alivePets.All(p => !p.CanDoMoreActions());
        }

        private async Task FeedPetAsync(Pet pet)
        {
            Menu.ShowActivityOptions("FEED");

            Console.Write("Choose food (1-4): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
            {
                var foodTypes = itemDatabase.GetFoodTypes();
                var foodNames = itemDatabase.GetFoodNames();
                
                await ExecuteActivityAsync(pet, "FEEDING", () => pet.ApplyFood(new Item(foodNames[choice - 1], foodTypes[choice - 1], 2)), 3);
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid choice!");
            }
        }

        private async Task PlayWithPetAsync(Pet pet)
        {
            var toyOptions = PetActivity.GetPlayOptions();
            Menu.ShowActivityOptions("PLAY");

            Console.Write($"Choose activity (1-{toyOptions.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= toyOptions.Count)
            {
                await ExecuteActivityAsync(pet, "PLAYING", () => pet.ApplyPlay(toyOptions[choice - 1]), 4);
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid choice!");
            }
        }

        private async Task PutPetToRestAsync(Pet pet)
        {
            var restTypes = PetActivity.GetRestTypes();
            Menu.ShowActivityOptions("REST");

            Console.Write($"Choose rest option (1-{restTypes.Length}): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= restTypes.Length)
            {
                await ExecuteActivityAsync(pet, "PUTTING TO REST", () => pet.ApplyRest(restTypes[choice - 1]), 5);
            }
            else
            {
                EventSystem.TriggerGameMessage("Invalid choice!");
            }
        }

        private async Task ExecuteActivityAsync(Pet pet, string activityType, Func<string> activity, int duration)
        {
            Console.Clear();
            Console.WriteLine($"=== {activityType} {pet.Name.ToUpper()} ===");
            
            for (int i = 0; i < duration; i++)
            {
                await Task.Delay(800);
                Console.Write(".");
            }
            Console.WriteLine();
            
            var response = activity();
            EventSystem.TriggerGameMessage(response);
            
            pet.IncrementActionCount();
            
            if (pet.IsAlive)
            {
                Console.WriteLine();
                Console.WriteLine($"Updated Status: {pet.GetCurrentStatus()}");
                
                if (CheckAllPetsCompletedActions())
                {
                    Console.WriteLine();
                    Console.WriteLine("*** All pets have completed their daily actions! ***");
                    await AdvanceToNextDay();
                    return;
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("*** Pet died during activity! ***");
                
                if (CheckAllPetsCompletedActions())
                {
                    Console.WriteLine("*** All remaining pets have completed their daily actions! ***");
                    await AdvanceToNextDay();
                    return;
                }
            }
        }

        private async Task AdvanceToNextDay()
        {
            petManager.ProcessEndOfDay();
        }
        #endregion

        #region Display Methods
        private void DisplayCreatorInfo()
        {
            Console.WriteLine("\n=== CREATOR INFORMATION ===");
            Console.WriteLine("Name: Ahmet Ali Kayaci");
            Console.WriteLine("Student Number: 225040105");
            Console.WriteLine("Project: Interactive Pet Simulator");
            Console.WriteLine("Course: DGD208 - Spring 2025");
            Console.WriteLine("Created with C# .NET");
        }

        private void DisplayMessage(string message)
        {
            Console.WriteLine($"\n>> {message}");
        }

        private void HandlePetAdopted(Pet pet)
        {
            EventSystem.TriggerGameMessage($"Congratulations! You've adopted {pet.Name} the {pet.Type}!");
        }
        #endregion
    }
}