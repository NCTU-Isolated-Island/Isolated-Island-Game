using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Client
{
    public class ClientItemManager : ItemManager
    {
        public ClientItemManager()
        {
            
        }

        public override void AddItem(Item item)
        {
            if (!ContainsItem(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
            }
            else
            {
                itemDictionary[item.ItemID].UpdateItem(item.ItemName, item.Description);
            }
        }

        public override Item FindItem(int itemID)
        {
            if (ContainsItem(itemID))
            {
                return itemDictionary[itemID];
            }
            else
            {
                Item item = new Item(itemID, "傳輸中", "傳輸中");
                AddItem(item);
                SystemManager.Instance.OperationManager.FetchDataResolver.FetchItem(itemID);
                return item;
            }
        }
    }
}
