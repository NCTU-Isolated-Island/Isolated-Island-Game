using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class InventoryItemInfoRepository
    {
        public abstract bool Create(int inventoryID, int itemID, int itemCount, int positionIndex, out InventoryItemInfo info);
        public abstract bool Read(int inventoryItemInfoID, out InventoryItemInfo info);
        public abstract void Update(InventoryItemInfo info, int inventoryID);
        public abstract void Delete(int inventoryItemInfoID);
        public abstract List<InventoryItemInfo> ListOfInventory(int inventoryID);
    }
}
