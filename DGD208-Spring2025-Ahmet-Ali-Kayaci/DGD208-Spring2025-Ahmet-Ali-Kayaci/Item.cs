namespace PetSimulator
{
    public class Item
    {
        #region Properties
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int UsageDuration { get; set; }
        #endregion

        #region Constructor
        public Item(string name, ItemType type, int usageDuration)
        {
            Name = name;
            Type = type;
            UsageDuration = usageDuration;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Name} ({Type}) - Duration: {UsageDuration}s";
        }
        #endregion
    }
}