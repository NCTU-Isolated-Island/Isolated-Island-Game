using IsolatedIslandGame.Protocol;
using System.Text;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class UnlockSpecificBlueprintQuestReward : QuestReward
    {
        public int BlueprintID { get; private set; }
        public override QuestRewardType QuestRewardType { get { return QuestRewardType.UnlockSpecificBlueprint; } }
        public override string Description
        {
            get
            {
                Blueprint blueprint;
                if (BlueprintManager.Instance.FindBlueprint(BlueprintID, out blueprint))
                {
                    return "解鎖藍圖： " + blueprint.ToString();
                }
                else
                {
                    return $"解鎖未知的藍圖";
                }
            }
        }

        public UnlockSpecificBlueprintQuestReward(int questRewardID, int blueprintID) : base(questRewardID)
        {
            BlueprintID = blueprintID;
        }

        public override void GiveReward(Player player)
        {
            Blueprint blueprint;
            if (BlueprintManager.Instance.FindBlueprint(BlueprintID, out blueprint))
            {
                player.GetBlueprint(blueprint);
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}
