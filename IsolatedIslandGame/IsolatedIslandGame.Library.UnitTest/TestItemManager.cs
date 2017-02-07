using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Library.UnitTest
{
    class TestItemManager : ItemManager
    {
        public override event Action<Item> OnItemUpdate;

        public override void AddItem(Item item)
        {
            if (!ContainsItem(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
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
                item = null;
                return false;
            }
        }

        public override bool SpecializeItemToMaterial(int itemID, out Material material)
        {
            throw new NotImplementedException();
        }
    }
}
