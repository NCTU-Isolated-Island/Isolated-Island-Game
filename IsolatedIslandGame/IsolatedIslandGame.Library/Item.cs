namespace IsolatedIslandGame.Library
{
    public class Item
    {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }

        public Item(int itemID, string itemName)
        {
            ItemID = itemID;
            ItemName = itemName;
        }
    }
}
