using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class GetSpecificItemQuestRequirement : QuestRequirement
    {
        public int SpecificItemID { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.GetSpecificItem;
            }
        }
        public override string Description
        {
            get
            {
                Item item;
                if(ItemManager.Instance.FindItem(SpecificItemID, out item))
                {
                    return $"獲得物品： {item.ItemName}";
                }
                else
                {
                    return $"獲得未知的物品";
                }
            }
        }

        public GetSpecificItemQuestRequirement(int questRequirementID, int specificItemID) : base(questRequirementID)
        {
            SpecificItemID = specificItemID;
        }
    }
}
