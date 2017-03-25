using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;
using System.Text;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificScoreRandomMaterialQuestReward : QuestReward
    {
        public int MaterialCount { get; private set; }
        public int MaterialScore { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificScoreRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"給予{MaterialScore}分的隨機素材 x{MaterialCount}";
            }
        }

        public GiveSpecificNumberSpecificScoreRandomMaterialQuestReward(int questRewardID, int materialCount, int materialScore) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialScore = materialScore;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Score == MaterialScore).ToArray();
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder stringBuilder = new StringBuilder("獲得了： ");
            for (int i = 0; i < MaterialCount; i++)
            {
                Material material = materials[randomGenerator.Next(0, materials.Length)];
                player.Inventory.AddItem(material, 1);
                stringBuilder.Append($"{material.ItemName} ");
            }
            player.User.EventManager.UserInform("提示", stringBuilder.ToString());
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}
