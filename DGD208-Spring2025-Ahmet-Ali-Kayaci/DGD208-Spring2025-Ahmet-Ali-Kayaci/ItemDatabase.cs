using System;
using System.Collections.Generic;

public class ItemDatabase
{
    public List<Item> Items { get; private set; }

    public ItemDatabase()
    {
        Items = new List<Item>();
        LoadItems();
    }

    private void LoadItems()
    {
        Items.Add(new Item("Fish", new Dictionary<PetStat, int> 
        {
            { PetStat.Fun, 10 },
            { PetStat.Fullness, 20 }
        }));

        Items.Add(new Item("Catnip", new Dictionary<PetStat, int>
        {
            { PetStat.Fun, 25 },
            { PetStat.Health, 5 }
        }));

        Items.Add(new Item("Bird Seed", new Dictionary<PetStat, int>
        {
            { PetStat.Fun, 15 },
            { PetStat.Fullness, 5 }
        }));

        Items.Add(new Item("Carrot", new Dictionary<PetStat, int>
        {
            { PetStat.Fun, 5 },
            { PetStat.Fullness, 25 }
        }));
    }

    public Item GetItemByName(string name)
    {
        return Items.Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
