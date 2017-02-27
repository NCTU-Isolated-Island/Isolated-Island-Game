using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class HaveSpecificNumberFriendQuestRequirement : QuestRequirement
    {
        public int SpecificFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.HaveSpecificNumberFriend;
            }
        }
        public override string Description
        {
            get
            {
                return $"擁有{SpecificFriendNumber}位朋友";
            }
        }

        public HaveSpecificNumberFriendQuestRequirement(int questRequirementID, int specificFriendNumber) : base(questRequirementID)
        {
            SpecificFriendNumber = specificFriendNumber;
        }
    }
}
