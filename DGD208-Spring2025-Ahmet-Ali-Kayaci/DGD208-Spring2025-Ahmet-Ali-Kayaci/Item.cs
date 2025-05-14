public class Item
{
    public string Name { get; set; }
    public Dictionary<PetStat, int> Effects { get; set; }

    public Item(string name, Dictionary<PetStat, int> effects)
    {
        Name = name;
        Effects = effects;
    }
}
