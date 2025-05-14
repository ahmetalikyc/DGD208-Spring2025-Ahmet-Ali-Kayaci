using System.Collections.Generic;

public static class ItemDatabase
{
    public static List<Item> items = new List<Item>
    {
        new Item("Cat Food", ItemType.Food, new PetType[] { PetType.Cat }, new Dictionary<PetStat, int>
        {
            { PetStat.Fullness, 20 },
            { PetStat.Fun, -2 },
            { PetStat.Health, 2 }
        }, 2),

        new Item("Bird Seed", ItemType.Food, new PetType[] { PetType.Parrot }, new Dictionary<PetStat, int>
        {
            { PetStat.Fullness, 15 },
            { PetStat.Fun, 0 },
            { PetStat.Health, 1 }
        }, 2),
    };

    public static List<Item> GetItemsForPet(PetType type)
    {
        return items.Where(i => i.applicablePets.Contains(type)).ToList();
    }
}
