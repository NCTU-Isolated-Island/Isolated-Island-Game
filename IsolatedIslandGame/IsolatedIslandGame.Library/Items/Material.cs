namespace IsolatedIslandGame.Library.Items
{
    public class Material : Item
    {
        public int MaterialID { get; private set; }
        public bool IsUsing { get; private set; }
        public Material(int itemID, string itemName, int materialID, bool isUsing) : base(itemID, itemName)
        {
            MaterialID = materialID;
            IsUsing = isUsing;
        }
    }
}
