namespace IsolatedIslandGame.Library.Items
{
    public abstract class InventoryItemInfoFactory
    {
        public static InventoryItemInfoFactory Instance { get; protected set; }
        public static void Initial(InventoryItemInfoFactory factory)
        {
            Instance = factory;
        }

        public abstract InventoryItemInfo CreateItemInfo(int inventoryID, int itemID, int count, int positionIndex, bool isUsing);
        public abstract void DeleteItemInfo(int inventoryItemInfoID);
    }
}
