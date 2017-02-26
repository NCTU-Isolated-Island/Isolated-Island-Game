using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int MaterialCount { get; private set; }
        [MessagePackMember(2)]
        public int MaterialScore { get; private set; }
        [MessagePackMember(3)]
        public GroupType GroupType { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"{GroupNameMapping.GetGroupName(GroupType)}陣營{MaterialScore}分的隨機素材 x{MaterialCount}";
            }
        }

        [MessagePackDeserializationConstructor]
        public GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward() { }
        public GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward(int questRewardID, int materialCount, int materialScore, GroupType groupType) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialScore = materialScore;
            GroupType = groupType;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Score == MaterialScore && x.GroupType == GroupType).ToArray();
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
