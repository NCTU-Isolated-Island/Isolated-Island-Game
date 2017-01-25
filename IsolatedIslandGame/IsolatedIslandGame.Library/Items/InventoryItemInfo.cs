using System;

namespace IsolatedIslandGame.Library.Items
{
    public class InventoryItemInfo
    {
        public int InventoryItemInfoID { get; private set; }
        public Item Item { get; private set; }

        private int count;
        public int Count { get { return count; } set { count = value; onInventoryItemInfoUpdate?.Invoke(this); } }
        private int positionIndex;
        public int PositionIndex { get { return positionIndex; } set { positionIndex = value; onInventoryItemInfoUpdate?.Invoke(this); } }
        private bool isFavorite;
        public bool IsFavorite { get { return isFavorite; } set { isFavorite = value; onInventoryItemInfoUpdate?.Invoke(this); } }

        private event Action<InventoryItemInfo> onInventoryItemInfoUpdate;
        public event Action<InventoryItemInfo> OnInventoryItemInfoUpdate { add { onInventoryItemInfoUpdate += value; } remove { onInventoryItemInfoUpdate -= value; } }

        public InventoryItemInfo(int inventoryItemInfoID, Item item, int count, int positionIndex, bool isFavorite)
        {
            InventoryItemInfoID = inventoryItemInfoID;
            Item = item;
            Count = count;
            PositionIndex = positionIndex;
            IsFavorite = isFavorite;

            ItemManager.Instance.OnItemUpdate += UpdateItem;
        }

        ~InventoryItemInfo()
        {
            ItemManager.Instance.OnItemUpdate -= UpdateItem;
        }

        private void UpdateItem(Item item)
        {
            if(item.ItemID == Item.ItemID)
            {
                Item = item;
            }
        }
    }
}
