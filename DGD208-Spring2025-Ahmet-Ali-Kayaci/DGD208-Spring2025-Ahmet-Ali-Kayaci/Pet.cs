public class Pet
{
    public string Name { get; set; }
    public PetType Type { get; set; }
    public Dictionary<PetStat, int> Stats { get; set; }

    public Pet(string name, PetType type)
    {
        Name = name;
        Type = type;
        Stats = new Dictionary<PetStat, int>
        {
            { PetStat.Health, 50 },
            { PetStat.Fun, 50 },
            { PetStat.Fullness, 50 },
            { PetStat.Sleep, 50 }
        };
    }

    public void ApplyItemEffects(Item item)
    {
        foreach (var effect in item.Effects)
        {
            Stats[effect.Key] += effect.Value;
            if (Stats[effect.Key] > 100) Stats[effect.Key] = 100;
            if (Stats[effect.Key] < 0) Stats[effect.Key] = 0;
        }
    }
}
