using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StatManager
{
    private List<Pet> pets;

    public StatManager(List<Pet> pets)
    {
        this.pets = pets;
    }

    public async Task UpdateStatsAsync()
    {
        while (pets.Any(p => p.isAlive))
        {
            foreach (var pet in pets)
            {
                if (pet.isAlive)
                {
                    UpdatePetStats(pet);
                    CheckPetStatus(pet);
                }
            }
            await Task.Delay(1000); // 1 saniyelik aralıklarla statları güncelle
        }
    }

    private void UpdatePetStats(Pet pet)
    {
        pet.stats[PetStat.Fullness] = Math.Clamp(pet.stats[PetStat.Fullness] - 1, 0, 100);
        pet.stats[PetStat.Energy] = Math.Clamp(pet.stats[PetStat.Energy] - 1, 0, 100);
        pet.stats[PetStat.Sleep] = Math.Clamp(pet.stats[PetStat.Sleep] - 1, 0, 100);
        pet.stats[PetStat.Fun] = Math.Clamp(pet.stats[PetStat.Fun] - 1, 0, 100);
        pet.stats[PetStat.Health] = Math.Clamp(pet.stats[PetStat.Health] - 1, 0, 100);
    }

    private void CheckPetStatus(Pet pet)
    {
        if (pet.stats.Values.Any(stat => stat == 0))
        {
            pet.Die();
            Console.WriteLine($"{pet.name} has died due to stat depletion!");
        }
    }
}
