using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public abstract class ItemManager
    {
        public static ItemManager Instance { get; private set; }
        public static void Initial(ItemManager manager)
        {
            Instance = manager;
        }

        protected Dictionary<int, Item> itemDictionary;
        public IEnumerable<Item> Items { get { return itemDictionary.Values; } }
        public int ItemCount { get { return itemDictionary.Count; } }

        protected ItemManager()
        {
            itemDictionary = new Dictionary<int, Item>();
        }
        public bool ContainsItem(int itemID)
        {
            return itemDictionary.ContainsKey(itemID);
        }
        public abstract Item FindItem(int itemID);
        public abstract void AddItem(Item item);
        public bool RemoveItem(int item)
        {
            return itemDictionary.Remove(item);
        }
    }
}
