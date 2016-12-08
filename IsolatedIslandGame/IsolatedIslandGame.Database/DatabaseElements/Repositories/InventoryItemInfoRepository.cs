using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class InventoryItemInfoRepository
    {
        public abstract InventoryItemInfo Create(int inventoryID, int itemID, int itemCount, int positionIndex);
        public abstract InventoryItemInfo Read(int inventoryItemInfoID);
        public abstract void Update(InventoryItemInfo info, int inventoryID);
        public abstract void Delete(int inventoryItemInfoID);
        public abstract List<InventoryItemInfo> ListOfInventory(int inventoryID);
    }
}
