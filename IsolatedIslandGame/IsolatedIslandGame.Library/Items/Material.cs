using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Items
{
    public class Material : Item
    {
        public int MaterialID { get; private set; }
        public int Score { get; private set; }
        public GroupType GroupType { get; private set; }
        public int Level { get; private set; }
        public Material(int itemID, string itemName, string description, int materialID, int score, GroupType groupType, int level) : base(itemID, itemName, description)
        {
            MaterialID = materialID;
            Score = score;
            GroupType = groupType;
            Level = level;
        }
    }
}
