using System;

namespace PetSimulator
{
    public static class StatManager
    {
        #region Mood System
        public static string GetMood(int happiness)
        {
            return happiness switch
            {
                >= 85 => "Great",
                >= 75 => "Happy", 
                >= 65 => "Not Bad",
                _ => "Unhappy"
            };
        }
        #endregion

        #region Display Methods
        public static void DisplayPetDetails(Pet pet, int currentDay)
        {
            Console.WriteLine($"\n=== {pet.Name} ({pet.Type}) ===");
            Console.WriteLine($"Happiness: {pet.GetStatValue(PetStat.Happiness)}/100");
            Console.WriteLine($"Hunger: {pet.GetStatValue(PetStat.Hunger)}/100");
            Console.WriteLine($"Sleep: {pet.GetStatValue(PetStat.Sleep)}/100");
            Console.WriteLine($"Energy: {pet.GetStatValue(PetStat.Energy)}/100");
            Console.WriteLine($"Age: {pet.GetAge(currentDay)} days");
            Console.WriteLine($"Status: {(pet.IsAlive ? "Alive" : "Deceased")}");
            Console.WriteLine($"Mood: {GetMood(pet.GetStatValue(PetStat.Happiness))}");
        }

        public static void DisplayPetSummary(Pet pet, int currentDay)
        {
            Console.WriteLine($"- {pet.Name} ({pet.Type}): H:{pet.GetStatValue(PetStat.Happiness)} Hu:{pet.GetStatValue(PetStat.Hunger)} S:{pet.GetStatValue(PetStat.Sleep)} E:{pet.GetStatValue(PetStat.Energy)} Age:{pet.GetAge(currentDay)}d Mood:{GetMood(pet.GetStatValue(PetStat.Happiness))}");
        }
        #endregion
    }
}