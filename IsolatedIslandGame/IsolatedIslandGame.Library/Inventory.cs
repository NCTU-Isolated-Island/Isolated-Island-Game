using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IsolatedIslandGame.Library
{
    public class Inventory
    {
        public static int DefaultCapacity { get { return 20; } }

        public int InventoryID { get; private set; }
        public int Capacity { get; private set; }
        private Dictionary<int, InventoryItemInfo> itemInfoDictionary;
        private InventoryItemInfo[] itemInfos;
        public int DifferentItemCount { get { return itemInfoDictionary.Count; } }
        

        public IEnumerable<InventoryItemInfo> ItemInfos { get { return itemInfoDictionary.Values.OrderBy(x => x.PositionIndex); } }

        public delegate void InventoryItemInfoChangeEventHandler(InventoryItemInfo info, DataChangeType changeType);
        private event InventoryItemInfoChangeEventHandler onItemInfoChange;
        public event InventoryItemInfoChangeEventHandler OnItemInfoChange { add { onItemInfoChange += value; } remove { onItemInfoChange -= value; } }

        public Inventory(int inventoryID, int capacity)
        {
            InventoryID = inventoryID;
            Capacity = capacity;
            itemInfoDictionary = new Dictionary<int, InventoryItemInfo>();
            itemInfos = new InventoryItemInfo[Capacity];
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
                itemInfos[info.PositionIndex] = info;
                onItemInfoChange?.Invoke(info, DataChangeType.Add);
            }
            else
            {
                InventoryItemInfo existedInfo = itemInfoDictionary[info.InventoryItemInfoID];
                existedInfo.Count = info.Count;
                existedInfo.PositionIndex = info.PositionIndex;
                onItemInfoChange?.Invoke(existedInfo, DataChangeType.Update);
            }
        }
        public void RemoveItemInfo(int itemInfoID)
        {
            if (ContainsInventoryItemInfo(itemInfoID))
            {
                InventoryItemInfo info = itemInfoDictionary[itemInfoID];
                itemInfoDictionary.Remove(itemInfoID);
                itemInfos[info.PositionIndex] = null;
                onItemInfoChange?.Invoke(info, DataChangeType.Remove);
            }
        }
        public bool AddItem(Item item, int count)
        {
            InventoryItemInfo info = FindInventoryItemInfoByItemID(item.ItemID);
            if (info == null)
            {
                int positionIndex = Array.FindIndex(itemInfos, x => x == null);
                if (positionIndex >= 0)
                {
                    info = InventoryItemInfoFactory.Instance?.CreateItemInfo(InventoryID, item.ItemID, count, positionIndex);
                    itemInfoDictionary.Add(info.InventoryItemInfoID, info);
                    itemInfos[info.PositionIndex] = info;
                    onItemInfoChange?.Invoke(info, DataChangeType.Add);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                info.Count += count;
                onItemInfoChange?.Invoke(info, DataChangeType.Update);
            }
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
                    onItemInfoChange?.Invoke(info, DataChangeType.Remove);
                }
                else
                {
                    onItemInfoChange?.Invoke(info, DataChangeType.Update);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
