using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.Quests
{
    public class QuestRecord
    {
        public Quest Quest { get; private set; }
        private List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();
        public IEnumerable<QuestRequirementRecord> RequirementRecords { get { return requirementRecords.ToArray(); } }

        public bool IsFinished { get { return requirementRecords.TrueForAll(x => x.IsSufficient); } }

        private event Action<QuestRecord> onQuestStatusChange;
        public event Action<QuestRecord> OnQuestStatusChange { add { onQuestStatusChange += value; } remove { onQuestStatusChange -= value; } }

        public QuestRecord(Quest quest, List<QuestRequirementRecord> requirementRecords)
        {
            Quest = quest;
            this.requirementRecords = requirementRecords;
            foreach(var requirementRecord in requirementRecords)
            {
                requirementRecord.OnRequirementStatusChange += (record) =>
                {
                    onQuestStatusChange?.Invoke(this);
                };
            }
        }
    }
}
