using IsolatedIslandGame.Protocol;
using System;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class FinishedInSpecificTimeSpanQuestRequirement : QuestRequirement
    {
        public TimeSpan SpecificTimeSpan { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.FinishedInSpecificTimeSpan;
            }
        }
        public override string Description
        {
            get
            {
                return $"在時間{SpecificTimeSpan}內完成";
            }
        }

        public FinishedInSpecificTimeSpanQuestRequirement(int questRequirementID, TimeSpan specificTimeSpan) : base(questRequirementID)
        {
            SpecificTimeSpan = specificTimeSpan;
        }
    }
}
