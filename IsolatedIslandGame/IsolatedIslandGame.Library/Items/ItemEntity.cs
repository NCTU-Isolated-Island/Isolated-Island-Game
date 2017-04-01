namespace IsolatedIslandGame.Library.Items
{
    public class ItemEntity
    {
        public int ItemEntityID { get; private set; }
        public int ItemID { get; private set; }
        public float PositionX { get; private set; }
        public float PositionZ { get; private set; }

        public ItemEntity(int itemEntityID, int itemID, float positionX, float positionZ)
        {
            ItemEntityID = itemEntityID;
            ItemID = itemID;
            PositionX = positionX;
            PositionZ = positionZ;
        }
    }
}
