﻿namespace IsolatedIslandGame.Library.Items
{
    public class InventoryItemInfo
    {
        public int InventoryItemInfoID { get; private set; }
        public Item Item { get; private set; }
        public int Count { get; set; }
        public int PositionIndex { get; set; }

        public InventoryItemInfo(int inventoryItemInfoID, Item item, int count, int positionIndex)
        {
            InventoryItemInfoID = inventoryItemInfoID;
            Item = item;
            Count = count;
            PositionIndex = positionIndex;
        }
    }
}
