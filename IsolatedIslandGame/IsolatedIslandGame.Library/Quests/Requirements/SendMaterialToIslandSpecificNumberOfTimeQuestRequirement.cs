using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMaterialToIslandSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMaterialToIslandSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"送素材到孤島上{SpecificNumberOfTime}次";
            }
        }

        public SendMaterialToIslandSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
