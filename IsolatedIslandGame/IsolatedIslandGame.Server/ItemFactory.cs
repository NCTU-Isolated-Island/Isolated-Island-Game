using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class ItemFactory : ItemManager
    {
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
            }
        }

        public override Item FindItem(int itemID)
        {
            if(ContainsItem(itemID))
            {
                return itemDictionary[itemID];
            }
            else
            {
                return null;
            }
        }
    }
}
