using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class InventoryRepository
    {
        public abstract bool Create(int playerID, int capacity, out Inventory inventory);
        public abstract bool Read(int inventoryID, out Inventory inventory);
        public abstract bool ReadByPlayerID(int playerID, out Inventory inventory);
        public abstract void Update(Inventory inventory);
        public abstract void Delete(int inventoryID);
    }
}
