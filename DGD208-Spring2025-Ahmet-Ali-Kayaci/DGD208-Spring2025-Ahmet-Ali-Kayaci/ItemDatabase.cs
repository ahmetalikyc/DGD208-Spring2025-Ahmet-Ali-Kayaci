using System.Collections.Generic;
using System.Linq;

namespace PetSimulator
{
    public class ItemDatabase
    {
        #region Fields
        private List<Item> items;
        #endregion

        #region Constructor
        public ItemDatabase()
        {
            InitializeItems();
        }
        #endregion

        #region Initialization
        private void InitializeItems()
        {
            items = new List<Item>
            {
                new Item("Fresh Carrots", ItemType.RabbitFood, 2),
                new Item("Gourmet Cat Food", ItemType.CatFood, 2),
                new Item("Premium Dog Food", ItemType.DogFood, 2),
                new Item("Premium Bird Seeds", ItemType.BirdFood, 2)
            };
        }
        #endregion

        #region LINQ Queries
        public List<Item> GetAllItems()
        {
            return items.ToList();
        }

        public Item GetItemByName(string name)
        {
            return items.FirstOrDefault(i => i.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public List<Item> GetFoodItems()
        {
            return items.Where(item => item.Type == ItemType.RabbitFood || 
                                       item.Type == ItemType.CatFood || 
                                       item.Type == ItemType.DogFood || 
                                       item.Type == ItemType.BirdFood).ToList();
        }

        public List<string> GetFoodOptionNames()
        {
            return new List<string> { "Carrots", "Cat Food", "Dog Food", "Bird Seeds" };
        }

        public ItemType[] GetFoodTypes()
        {
            return new[] { ItemType.RabbitFood, ItemType.CatFood, ItemType.DogFood, ItemType.BirdFood };
        }

        public string[] GetFoodNames()
        {
            return new[] { "Fresh Carrots", "Gourmet Cat Food", "Premium Dog Food", "Premium Bird Seeds" };
        }
        #endregion
    }
}