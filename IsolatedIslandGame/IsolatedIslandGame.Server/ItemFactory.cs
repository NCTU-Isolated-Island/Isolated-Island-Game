using System;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class ItemFactory : ItemManager
    {
        private event Action<Item> onItemUpdate;
        public override event Action<Item> OnItemUpdate { add { onItemUpdate += value; } remove { onItemUpdate -= value; } }

        public ItemFactory()
        {
            var items = DatabaseService.RepositoryList.ItemRepository.ListAll();
            foreach(var item in items)
            {
                AddItem(item);
            }
        }

        public override void AddItem(Item item)
        {
            if(!ContainsItem(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
                onItemUpdate?.Invoke(item);
            }
        }

        public override bool FindItem(int itemID, out Item item)
        {
            if(ContainsItem(itemID))
            {
                item = itemDictionary[itemID];
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }
    }
}
