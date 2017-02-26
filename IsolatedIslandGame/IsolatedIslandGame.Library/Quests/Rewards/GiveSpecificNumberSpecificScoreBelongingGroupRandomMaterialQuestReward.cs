using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int MaterialCount { get; private set; }
        [MessagePackMember(2)]
        public int MaterialScore { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"給予所屬陣營{MaterialScore}分的隨機素材 x{MaterialCount}";
            }
        }

        [MessagePackDeserializationConstructor]
        public GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward() { }
        public GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward(int questRewardID, int materialCount, int materialScore) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialScore = materialScore;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Score == MaterialScore && x.GroupType == player.GroupType).ToArray();
            Random randomGenerator = new Random();
            for(int i = 0;i < MaterialCount; i++)
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
