using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Server.Items
{
    public class ServerInventoryItemInfoFactory : InventoryItemInfoFactory
    {
        public override bool CreateItemInfo(int inventoryID, int itemID, int count, int positionIndex, out InventoryItemInfo info)
        {
            return DatabaseService.RepositoryList.InventoryItemInfoRepository.Create(
                inventoryID: inventoryID,
                itemID: itemID,
                itemCount: count,
                positionIndex: positionIndex,
                isFavorite: false,
                info: out info );
        }

        public override void DeleteItemInfo(int inventoryItemInfoID)
        {
            DatabaseService.RepositoryList.InventoryItemInfoRepository.Delete(inventoryItemInfoID);
        }
    }
}
