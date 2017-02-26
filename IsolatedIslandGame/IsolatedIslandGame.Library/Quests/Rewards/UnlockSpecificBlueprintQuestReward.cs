using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;
using System.Text;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class UnlockSpecificBlueprintQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int BlueprintID { get; private set; }
        public override QuestRewardType QuestRewardType { get { return QuestRewardType.UnlockSpecificBlueprint; } }
        public override string Description
        {
            get
            {
                Blueprint blueprint;
                if (BlueprintManager.Instance.FindBlueprint(BlueprintID, out blueprint))
                {
                    StringBuilder stringBuilder = new StringBuilder("解鎖藍圖： ");
                    Item item;
                    foreach(var info in blueprint.Requirements)
                    {
                        if (ItemManager.Instance.FindItem(info.itemID, out item))
                        {
                            stringBuilder.Append($"{item.ItemName} x{info.itemCount} ");
                        }
                        else
                        {
                            stringBuilder.Append($"未知的物品 x{info.itemCount} ");
                        }
                    }
                    stringBuilder.Append("=> ");
                    foreach (var info in blueprint.Products)
                    {
                        if (ItemManager.Instance.FindItem(info.itemID, out item))
                        {
                            stringBuilder.Append($"{item.ItemName} x{info.itemCount} ");
                        }
                        else
                        {
                            stringBuilder.Append($"未知的物品 x{info.itemCount} ");
                        }
                    }
                    return stringBuilder.ToString();
                }
                else
                {
                    return $"解鎖未知的藍圖";
                }
            }
        }

        [MessagePackDeserializationConstructor]
        public UnlockSpecificBlueprintQuestReward() { }
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
