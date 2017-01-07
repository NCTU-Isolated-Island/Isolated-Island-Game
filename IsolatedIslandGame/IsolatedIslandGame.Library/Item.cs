using System;

namespace IsolatedIslandGame.Library
{
    public class Item
    {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public string Description { get; private set; }

        public Item(int itemID, string itemName, string description)
        {
            ItemID = itemID;
            ItemName = itemName;
            Description = description;
        }
    }
}
