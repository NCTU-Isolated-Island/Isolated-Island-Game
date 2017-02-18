using IsolatedIslandGame.Library;
using System;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Client
{
    public class ClientItemManager : ItemManager
    {
        private event Action<Item> onItemUpdate;
        public override event Action<Item> OnItemUpdate { add { onItemUpdate += value; } remove { onItemUpdate -= value; } }

        public override void AddItem(Item item)
        {
            if (!ContainsItem(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
            }
            else
            {
                itemDictionary[item.ItemID] = item;
                if(onItemUpdate != null)
                {
                    onItemUpdate.Invoke(item);
                }
            }
        }

        public override bool FindItem(int itemID, out Item item)
        {
            if (ContainsItem(itemID))
            {
                item = itemDictionary[itemID];
                return true;
            }
            else
            {
                item = new Item(itemID, "傳輸中", "傳輸中");
                AddItem(item);
                SystemManager.Instance.OperationManager.FetchDataResolver.FetchItem(itemID);
                return true;
            }
        }

        public override bool SpecializeItemToMaterial(int itemID, out Material material)
        {
            if (ContainsItem(itemID))
            {
                Item item = itemDictionary[itemID];
                if (item is Material)
                {
                    material = item as Material;
                }
                else
                {
                    material = new Material(item.ItemID, item.ItemName, item.Description, 0, 0);
                    itemDictionary[itemID] = material;
                    if (onItemUpdate != null)
                    {
                        onItemUpdate.Invoke(material);
                    }
                }
                return true;
            }
            else
            {
                material = null;
                return false;
            }
        }
    }
}
