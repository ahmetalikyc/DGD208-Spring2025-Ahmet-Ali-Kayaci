using System;
using System.Collections.Generic;

namespace PetSimulator
{
    public static class PetActivity
    {
        #region Activity Options
        public static List<string> GetPlayOptions()
        {
            return new List<string> { "Throw Ball", "Feather Wand Play", "Talk and Sing", "Tunnel Exploration" };
        }

        public static List<string> GetRestOptions()
        {
            return new List<string> { "Put in Dog House", "Warm Cushion Rest", "Cover Cage for Sleep", "Tunnel Hideaway" };
        }

        public static string[] GetRestTypes()
        {
            return new[] { "Dog House", "Warm Cushion", "Covered Cage", "Tunnel" };
        }
        #endregion

        #region Food Appropriateness
        public static bool IsFoodAppropriate(PetType petType, ItemType foodType)
        {
            return petType switch
            {
                PetType.Dog => foodType == ItemType.DogFood,
                PetType.Cat => foodType == ItemType.CatFood,
                PetType.Parrot => foodType == ItemType.BirdFood,
                PetType.Rabbit => foodType == ItemType.RabbitFood,
                _ => false
            };
        }

        public static string GetFoodRejectionResponse(Pet pet, Item food)
        {
            return pet.Type switch
            {
                PetType.Dog => $"{pet.Name} sniffs the {food.Name} and walks away disappointed. Dogs prefer dog food!",
                PetType.Cat => $"{pet.Name} sniffs delicately at the {food.Name} then sits down, clearly unimpressed. Cats know what they want!",
                PetType.Parrot => $"{pet.Name} tilts head curiously at the {food.Name} but won't touch it. Birds have specific dietary needs!",
                PetType.Rabbit => $"{pet.Name} twitches nose at the {food.Name} and bounds away sadly. Rabbits love their natural foods!",
                _ => $"{pet.Name} doesn't want the {food.Name}."
            };
        }
        #endregion

        #region Play Activity Results
        public static (bool CanPlay, int HappinessBonus, int EnergyChange, int SleepChange, string Message) GetPlayResult(Pet pet, string playType)
        {
            var responses = new Dictionary<(string, PetType), (bool, int, int, int, string)>
            {
                [("Throw Ball", PetType.Dog)] = (true, 25, -15, -10, $"{pet.Name} races after the ball with pure excitement! This is what dogs live for!"),
                [("Throw Ball", PetType.Cat)] = (false, 0, 0, 0, $"{pet.Name} gives the ball a disdainful look and sits down. Cats have better things to do!"),
                [("Throw Ball", PetType.Parrot)] = (false, 0, 0, 0, $"{pet.Name} observes the ball briefly then loses interest. Parrots need interactive activities!"),
                [("Throw Ball", PetType.Rabbit)] = (false, 0, 0, 0, $"{pet.Name} sniffs the ball cautiously then hops to safety. Rabbits feel safer with tunnels!"),
                
                [("Feather Wand Play", PetType.Cat)] = (true, 25, -15, -10, $"{pet.Name} purrs and pounces excitedly at the feather! Cats love hunting games!"),
                [("Feather Wand Play", PetType.Dog)] = (false, 0, 0, 0, $"{pet.Name} sniffs the feather but isn't interested. Dogs prefer ball games!"),
                [("Feather Wand Play", PetType.Parrot)] = (false, 0, 0, 0, $"{pet.Name} seems confused by the feather wand. Parrots prefer talking and puzzles!"),
                [("Feather Wand Play", PetType.Rabbit)] = (false, 0, 0, 0, $"{pet.Name} gets startled by the moving feather. Rabbits prefer calm activities!"),
                
                [("Talk and Sing", PetType.Parrot)] = (true, 25, -15, -10, $"{pet.Name} chirps and mimics happily! Parrots love mental stimulation and interaction!"),
                [("Talk and Sing", PetType.Dog)] = (false, 0, 0, 0, $"{pet.Name} tilts head but doesn't understand talking games. Dogs prefer physical activities!"),
                [("Talk and Sing", PetType.Cat)] = (false, 0, 0, 0, $"{pet.Name} meows once then walks away. Cats ignore talking!"),
                [("Talk and Sing", PetType.Rabbit)] = (false, 0, 0, 0, $"{pet.Name} twitches ears but seems uninterested. Rabbits prefer exploring activities!"),
                
                [("Tunnel Exploration", PetType.Rabbit)] = (true, 25, -15, -10, $"{pet.Name} hops excitedly through the tunnels! Rabbits love exploring and hiding!"),
                [("Tunnel Exploration", PetType.Dog)] = (false, 0, 0, 0, $"{pet.Name} is too big for tunnel exploration. Dogs prefer open space activities!"),
                [("Tunnel Exploration", PetType.Cat)] = (false, 0, 0, 0, $"{pet.Name} peeks into the tunnel but prefers stalking games. Cats like different activities!"),
                [("Tunnel Exploration", PetType.Parrot)] = (false, 0, 0, 0, $"{pet.Name} can't navigate the tunnels properly. Parrots prefer high perches and talking!")
            };

            return responses.TryGetValue((playType, pet.Type), out var result) ? result : (false, 0, 0, 0, $"{pet.Name} doesn't know how to play this game!");
        }
        #endregion

        #region Rest Activity Results
        public static (bool CanSleep, int HappinessBonus, string Message) GetRestResult(Pet pet, string restType)
        {
            var responses = new Dictionary<(string, PetType), (bool, int, string)>
            {
                [("Dog House", PetType.Dog)] = (true, 15, $"{pet.Name} curls up comfortably in the dog house, feeling safe and secure!"),
                [("Dog House", PetType.Cat)] = (true, 10, $"{pet.Name} finds the dog house a bit big but manages to sleep okay."),
                [("Dog House", PetType.Parrot)] = (false, 0, $"{pet.Name} feels trapped at ground level and can't see around. Parrots need high perches to feel safe!"),
                [("Dog House", PetType.Rabbit)] = (false, 0, $"{pet.Name} feels uncomfortable in the dog house. This place is not suitable for rabbits!"),
                
                [("Warm Cushion", PetType.Cat)] = (true, 15, $"{pet.Name} purrs contentedly on the warm cushion, kneading with tiny paws!"),
                [("Warm Cushion", PetType.Dog)] = (true, 10, $"{pet.Name} finds the cushion a bit small but sleeps comfortably enough."),
                [("Warm Cushion", PetType.Parrot)] = (false, 0, $"{pet.Name} feels unsafe on the low cushion and keeps looking around nervously. Parrots prefer elevated spots!"),
                [("Warm Cushion", PetType.Rabbit)] = (false, 0, $"{pet.Name} feels exposed on the warm cushion. This place is not suitable for rabbits!"),
                
                [("Covered Cage", PetType.Parrot)] = (true, 15, $"{pet.Name} settles down quietly as the cage cover creates a cozy, dark environment!"),
                [("Covered Cage", PetType.Dog)] = (false, 0, $"{pet.Name} feels trapped and anxious in the covered cage. This place is not suitable for dogs!"),
                [("Covered Cage", PetType.Cat)] = (false, 0, $"{pet.Name} feels claustrophobic in the covered cage. This place is not suitable for cats!"),
                [("Covered Cage", PetType.Rabbit)] = (false, 0, $"{pet.Name} feels confined in the covered cage. This place is not suitable for rabbits!"),
                
                [("Tunnel", PetType.Rabbit)] = (true, 15, $"{pet.Name} loves the cozy tunnel! Rabbits feel safest in enclosed spaces!"),
                [("Tunnel", PetType.Dog)] = (false, 0, $"{pet.Name} feels claustrophobic in the tunnel. This place is not suitable for dogs!"),
                [("Tunnel", PetType.Cat)] = (false, 0, $"{pet.Name} feels uncomfortable in the tunnel. This place is not suitable for cats!"),
                [("Tunnel", PetType.Parrot)] = (false, 0, $"{pet.Name} can't move properly in the tunnel. This place is not suitable for parrots!")
            };

            return responses.TryGetValue((restType, pet.Type), out var result) ? result : (false, 0, $"{pet.Name} can't sleep here!");
        }
        #endregion
    }
}