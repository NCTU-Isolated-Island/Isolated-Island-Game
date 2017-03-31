using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.Items
{
    public class ItemEntityManager
    {
        public static ItemEntityManager Instance { get; private set; }

        public static void Initial(ItemEntityManager itemEntityManager)
        {
            Instance = itemEntityManager;
        }

        protected Dictionary<int, ItemEntity> itemEntityDictionary = new Dictionary<int, ItemEntity>();
        public IEnumerable<ItemEntity> ItemEntities { get { return itemEntityDictionary.Values; } }
        public int ItemEntityCount { get { return itemEntityDictionary.Count; } }

        public delegate void ItemEntityChangeEventHandler(DataChangeType changeType, ItemEntity itemEntity);
        private event ItemEntityChangeEventHandler onItemEntityChange;
        public event ItemEntityChangeEventHandler OnItemEntityChange;

        public bool ContainsItemEntity(int itemEntityID)
        {
            return itemEntityDictionary.ContainsKey(itemEntityID);
        }
        public bool FindItemEntity(int itemEntityID, out ItemEntity itemEntity)
        {
            lock(itemEntityDictionary)
            {
                if (ContainsItemEntity(itemEntityID))
                {
                    itemEntity = itemEntityDictionary[itemEntityID];
                    return true;
                }
                else
                {
                    itemEntity = null;
                    return false;
                }
            }
        }
        public void AddItemEntity(ItemEntity itemEntity)
        {
            lock(itemEntityDictionary)
            {
                if (!ContainsItemEntity(itemEntity.ItemEntityID))
                {
                    itemEntityDictionary.Add(itemEntity.ItemEntityID, itemEntity);
                    onItemEntityChange?.Invoke(DataChangeType.Add, itemEntity);
                }
            }
        }
        public bool RemoveItemEntity(int itemEntityID)
        {
            lock (itemEntityDictionary)
            {
                if (ContainsItemEntity(itemEntityID))
                {
                    ItemEntity itemEntity = itemEntityDictionary[itemEntityID];
                    onItemEntityChange?.Invoke(DataChangeType.Remove, itemEntity);
                    return itemEntityDictionary.Remove(itemEntityID);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
