using System;

namespace IsolatedIslandGame.Library
{
    public class Item
    {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public string Description { get; private set; }

        private event Action<Item> onItemUpdate;
        public event Action<Item> OnItemUpdate { add { onItemUpdate += value; } remove { onItemUpdate -= value; } }

        public Item(int itemID, string itemName, string description)
        {
            ItemID = itemID;
            ItemName = itemName;
            Description = description;
        }
        public void UpdateItem(string itemName, string description)
        {
            ItemName = itemName;
            Description = description;
            onItemUpdate?.Invoke(this);
        }
    }
}
