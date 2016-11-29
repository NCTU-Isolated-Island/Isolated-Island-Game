namespace IsolatedIslandGame.Library.Items
{
    public abstract class InventoryItemInfoFactory
    {
        public static InventoryItemInfoFactory Instance { get; protected set; }

        public abstract InventoryItemInfo CreateItemInfo(Item item, int count, int positionIndex);
        public abstract void DeleteItemInfo(InventoryItemInfo itemInfo);
    }
}
