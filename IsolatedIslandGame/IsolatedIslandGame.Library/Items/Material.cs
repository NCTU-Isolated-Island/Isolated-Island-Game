namespace IsolatedIslandGame.Library.Items
{
    public class Material : Item
    {
        public int MaterialID { get; private set; }
        public Material(int itemID, string itemName, string description, int materialID) : base(itemID, itemName, description)
        {
            MaterialID = materialID;
        }
    }
}
