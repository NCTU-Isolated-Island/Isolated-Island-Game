using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class HaveSpecificNumberSpecificItemQuestRequirement : QuestRequirement
    {
        public int SpecificNumber { get; private set; }
        public int SpecificItemID { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.HaveSpecificNumberSpecificItem;
            }
        }
        public override string Description
        {
            get
            {
                Item item;
                if(ItemManager.Instance.FindItem(SpecificItemID, out item))
                {
                    return $"擁有{SpecificNumber}個{item.ItemName}";
                }
                else
                {
                    return $"擁有{SpecificNumber}個未知的物品";
                }
            }
        }

        public HaveSpecificNumberSpecificItemQuestRequirement(int questRequirementID, int specificNumber, int specificItemID) : base(questRequirementID)
        {
            SpecificNumber = specificNumber;
            SpecificItemID = specificItemID;
        }
    }
}
