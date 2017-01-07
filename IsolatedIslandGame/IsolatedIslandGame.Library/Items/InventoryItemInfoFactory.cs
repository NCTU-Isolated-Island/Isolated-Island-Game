namespace IsolatedIslandGame.Library.Items
{
    public abstract class InventoryItemInfoFactory
    {
        public static InventoryItemInfoFactory Instance { get; private set; }
        public static void Initial(InventoryItemInfoFactory factory)
        {
            Instance = factory;
        }

        public abstract bool CreateItemInfo(int inventoryID, int itemID, int count, int positionIndex, out InventoryItemInfo info);
        public abstract void DeleteItemInfo(int inventoryItemInfoID);
    }
}
