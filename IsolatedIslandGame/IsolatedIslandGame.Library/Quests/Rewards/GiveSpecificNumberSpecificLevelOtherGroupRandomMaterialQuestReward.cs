using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificLevelOtherGroupRandomMaterialQuestReward : QuestReward
    {
        public int MaterialCount { get; private set; }
        public int MaterialLevel { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificLevelOtherGroupRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"給予其他陣營{MaterialLevel}階的隨機素材 x{MaterialCount}";
            }
        }

        public GiveSpecificNumberSpecificLevelOtherGroupRandomMaterialQuestReward(int questRewardID, int materialCount, int materialLevel) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialLevel = materialLevel;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == MaterialLevel && x.GroupType != player.GroupType).ToArray();
            Random randomGenerator = new Random();
            for (int i = 0; i < MaterialCount; i++)
            {
                player.Inventory.AddItem(materials[randomGenerator.Next(0, materials.Length)], 1);
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}
