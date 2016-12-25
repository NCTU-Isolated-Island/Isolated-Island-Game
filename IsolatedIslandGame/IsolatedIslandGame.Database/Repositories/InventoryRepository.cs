using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class InventoryRepository
    {
        public abstract Inventory Create(int playerID, int capacity);
        public abstract Inventory Read(int inventoryID);
        public abstract Inventory ReadByPlayerID(int playerID);
        public abstract void Update(Inventory inventory);
        public abstract void Delete(int inventoryID);
    }
}
