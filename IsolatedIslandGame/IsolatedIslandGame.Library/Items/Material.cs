namespace IsolatedIslandGame.Library.Items
{
    public class Material : Item
    {
        public int MaterialID { get; private set; }
        public int Score { get; private set; }
        public Material(int itemID, string itemName, string description, int materialID, int score) : base(itemID, itemName, description)
        {
            MaterialID = materialID;
            Score = score;
        }
    }
}
