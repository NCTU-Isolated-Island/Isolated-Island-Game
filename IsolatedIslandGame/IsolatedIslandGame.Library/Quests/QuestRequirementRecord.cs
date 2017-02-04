using System;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirementRecord
    {
        protected Player player;
        public QuestRequirement Requirement { get; private set; }
        public abstract string ProgressStatus { get; }
        public abstract bool IsSufficient { get; }
        public abstract event Action<QuestRequirementRecord> OnRequirementStatusChange;

        protected QuestRequirementRecord(Player player, QuestRequirement requirement)
        {
            this.player = player;
            Requirement = requirement;
        }
    }
}
