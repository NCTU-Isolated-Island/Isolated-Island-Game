using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Server.Items
{
    public class ServerInventoryItemInfoFactory : InventoryItemInfoFactory
    {
        public override InventoryItemInfo CreateItemInfo(int inventoryID, int itemID, int count, int positionIndex)
        {
            return DatabaseService.RepositoryList.InventoryItemInfoRepository.Create(
                inventoryID: inventoryID,
                itemID: itemID,
                itemCount: count,
                positionIndex: positionIndex);
        }

        public override void DeleteItemInfo(int inventoryItemInfoID)
        {
            DatabaseService.RepositoryList.InventoryItemInfoRepository.Delete(inventoryItemInfoID);
        }
    }
}
