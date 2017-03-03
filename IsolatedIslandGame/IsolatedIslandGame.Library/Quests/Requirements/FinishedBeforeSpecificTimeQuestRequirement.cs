using IsolatedIslandGame.Protocol;
using System;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class FinishedBeforeSpecificTimeQuestRequirement : QuestRequirement
    {
        public DateTime SpecificTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.FinishedBeforeSpecificTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"在{SpecificTime}前完成";
            }
        }

        public FinishedBeforeSpecificTimeQuestRequirement(int questRequirementID, DateTime specificTime) : base(questRequirementID)
        {
            SpecificTime = specificTime;
        }
    }
}
