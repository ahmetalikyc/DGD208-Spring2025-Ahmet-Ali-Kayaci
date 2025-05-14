public class StatManager
{
    public async Task DecreaseStatsOverTime(Pet pet)
    {
        while (pet.Stats[PetStat.Health] > 0 && pet.Stats[PetStat.Fun] > 0 && pet.Stats[PetStat.Fullness] > 0 && pet.Stats[PetStat.Sleep] > 0)
        {
            await Task.Delay(1000); // 1 sn
            pet.Stats[PetStat.Health]--;
            pet.Stats[PetStat.Fun]--;
            pet.Stats[PetStat.Fullness]--;
            pet.Stats[PetStat.Sleep]--;

            if (pet.Stats[PetStat.Health] <= 0 || pet.Stats[PetStat.Fun] <= 0 || pet.Stats[PetStat.Fullness] <= 0 || pet.Stats[PetStat.Sleep] <= 0)
            {
                Console.WriteLine($"{pet.Name} has died!");
                break;
            }
        }
    }
}
