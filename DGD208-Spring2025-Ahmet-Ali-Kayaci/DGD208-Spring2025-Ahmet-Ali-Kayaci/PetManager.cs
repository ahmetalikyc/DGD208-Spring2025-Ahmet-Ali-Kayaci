using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PetSimulator
{
    public class PetManager
    {
        #region Fields
        private List<Pet> managedPets;
        private HashSet<Pet> reportedDeadPets;
        private int currentDay;
        private DateTime gameStartTime;
        #endregion

        #region Events
        public event Action<Pet> OnPetDied;
        #endregion

        #region Constructor
        public PetManager()
        {
            managedPets = new List<Pet>();
            reportedDeadPets = new HashSet<Pet>();
            currentDay = 1;
            gameStartTime = DateTime.Now;
        }
        #endregion

        #region Pet Management
        public void AddPet(Pet pet)
        {
            managedPets.Add(pet);
            EventSystem.TriggerGameMessage($"Pet {pet.Name} added to management system.");
        }

        public void RemovePet(Pet pet)
        {
            managedPets.Remove(pet);
        }
        #endregion

        #region LINQ Queries
        public List<Pet> GetAllPets()
        {
            return managedPets.ToList();
        }

        public List<Pet> GetAlivePets()
        {
            return managedPets.Where(p => p.IsAlive).ToList();
        }

        public List<Pet> GetDeadPets()
        {
            return managedPets.Where(p => !p.IsAlive).ToList();
        }

        public bool HasAnyPets()
        {
            return managedPets.Any();
        }

        public bool HasAlivePets()
        {
            return managedPets.Any(p => p.IsAlive);
        }
        #endregion

        #region Day Management
        public int GetCurrentDay()
        {
            return currentDay;
        }

        public void NextDay()
        {
            currentDay++;
            EventSystem.TriggerDayChanged(currentDay);
        }

        public void ProcessEndOfDay()
        {
            Console.Clear();
            Console.WriteLine("=== END OF DAY REPORT ===");
            Console.WriteLine("=========================");
            
            var alivePetsAtStart = GetAlivePets();
            
            foreach (var pet in alivePetsAtStart)
                pet.ApplyEndOfDayEffects();
            
            var todaysDeadPets = GetDeadPets().Where(p => !reportedDeadPets.Contains(p)).ToList();
            
            if (todaysDeadPets.Count > 0)
            {
                Console.WriteLine("\n*** DAILY REPORT - PET DEATHS ***");
                foreach (var deadPet in todaysDeadPets)
                {
                    Console.WriteLine($" {deadPet.Name} ({deadPet.Type}) died from {deadPet.GetDeathCause()}");
                    reportedDeadPets.Add(deadPet);
                }
                Console.WriteLine();
            }
            
            NextDay();
            
            foreach (var pet in GetAlivePets())
                pet.ResetDailyUsage();

            Console.WriteLine($"*** Day completed! Day {currentDay} begins! ***");
            Console.WriteLine("*** Note: Happiness is limited to 65 when any other stat drops below 65. ***");
            
            Console.WriteLine("\nPress any key to continue to the next day...");
            Console.ReadKey();
            Console.Clear();
        }

        public void DisplayAllPets()
        {
            if (!HasAnyPets())
            {
                EventSystem.TriggerGameMessage("You don't have any pets yet!");
                return;
            }

            Console.WriteLine("\n=== ALL YOUR PETS ===");
            var alivePets = GetAlivePets();
            var deadPets = GetDeadPets();

            if (alivePets.Any())
            {
                Console.WriteLine("\nAlive Pets:");
                alivePets.ForEach(pet => StatManager.DisplayPetSummary(pet, currentDay));
            }

            if (deadPets.Any())
            {
                Console.WriteLine("\nDeceased Pets:");
                deadPets.ForEach(pet => Console.WriteLine($"- {pet.Name} ({pet.Type}) - R.I.P."));
            }
        }
        #endregion
    }
}