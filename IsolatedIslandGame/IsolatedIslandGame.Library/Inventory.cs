using IsolatedIslandGame.Library.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public class Inventory
    {
        public static int DefaultCapacity { get { return 20; } }

        public int InventoryID { get; private set; }
        public int Capacity { get; private set; }
        private Dictionary<int, InventoryItemInfo> itemInfoDictionary;
        private List<InventoryItemInfo> itemInfos;
        public int DifferentItemCount { get { return itemInfoDictionary.Count; } }
        

        public IEnumerable<InventoryItemInfo> ItemInfos { get { return itemInfoDictionary.Values.OrderBy(x => x.PositionIndex); } }

        private event Action<InventoryItemInfo> onItemChange;
        public event Action<InventoryItemInfo> OnItemChange { add { onItemChange += value; } remove { onItemChange -= value; } }

        public Inventory(int inventoryID, int capacity)
        {
            InventoryID = inventoryID;
            Capacity = capacity;
            itemInfoDictionary = new Dictionary<int, InventoryItemInfo>();
            itemInfos = new List<InventoryItemInfo>(Capacity);
        }
        public bool ContainsInventoryItemInfo(int inventoryItemInfoID)
        {
            return itemInfoDictionary.ContainsKey(inventoryItemInfoID);
        }
        public bool ContainsItem(int itemID)
        {
            return itemInfoDictionary.Values.Any(x => x.Item.ItemID == itemID);
        }
        public InventoryItemInfo FindInventoryItemInfo(int inventoryItemInfoID)
        {
            if (ContainsInventoryItemInfo(inventoryItemInfoID))
            {
                return itemInfoDictionary[inventoryItemInfoID];
            }
            else
            {
                return null;
            }
        }
        public InventoryItemInfo FindInventoryItemInfoByItemID(int itemID)
        {
            if (ContainsItem(itemID))
            {
                return ItemInfos.First(x => x.Item.ItemID == itemID);
            }
            else
            {
                return null;
            }
        }
        public int ItemCount(int itemID)
        {
            if(ContainsItem(itemID))
            {
                return ItemInfos.First(x => x.Item.ItemID == itemID).Count;
            }
            else
            {
                return 0;
            }
        }
        public void LoadItemInfo(InventoryItemInfo info)
        {
            if(!ContainsInventoryItemInfo(info.InventoryItemInfoID))
            {
                itemInfoDictionary.Add(info.InventoryItemInfoID, info);
                onItemChange?.Invoke(info);
            }
        }
        public bool AddItem(Item item, int count)
        {
            InventoryItemInfo info = FindInventoryItemInfoByItemID(item.ItemID);
            if (info == null)
            {
                int positionIndex = itemInfos.FindIndex(x => x == null);
                info = InventoryItemInfoFactory.Instance?.CreateItemInfo(InventoryID, item.ItemID, count, positionIndex);
                itemInfoDictionary.Add(info.InventoryItemInfoID, info);
            }
            else
            {
                info.Count += count;
            }
            itemInfos[info.PositionIndex] = info;
            onItemChange?.Invoke(info);
            return true;
        }
        public bool RemoveItem(int itemID, int count)
        {
            if (ContainsItem(itemID) && ItemCount(itemID) >= count)
            {
                InventoryItemInfo info = FindInventoryItemInfoByItemID(itemID);
                info.Count -= count;
                if (info.Count == 0)
                {
                    if (itemInfoDictionary.ContainsKey(info.InventoryItemInfoID))
                    {
                        itemInfoDictionary.Remove(info.InventoryItemInfoID);
                    }
                    itemInfos[info.PositionIndex] = null;
                    InventoryItemInfoFactory.Instance?.DeleteItemInfo(info.InventoryItemInfoID);
                }
                onItemChange?.Invoke(info);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
