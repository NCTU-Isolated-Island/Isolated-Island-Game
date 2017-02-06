using System;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public class QuestRecord
    {
        public int QuestRecordID { get; private set; }
        public int PlayerID { get; private set; }
        public Quest Quest { get; private set; }
        private List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();
        public IEnumerable<QuestRequirementRecord> RequirementRecords { get { return requirementRecords.ToArray(); } }

        public bool IsFinished { get { return requirementRecords.TrueForAll(x => x.IsSufficient); } }

        private event Action<QuestRecord> onQuestRecordStatusChange;
        public event Action<QuestRecord> OnQuestRecordStatusChange { add { onQuestRecordStatusChange += value; } remove { onQuestRecordStatusChange -= value; } }

        [MessagePackDeserializationConstructor]
        public QuestRecord() { }
        public QuestRecord(int questRecordID, int playerID, Quest quest, List<QuestRequirementRecord> requirementRecords)
        {
            QuestRecordID = questRecordID;
            PlayerID = playerID;
            Quest = quest;
            this.requirementRecords = requirementRecords;
            foreach(var requirementRecord in requirementRecords)
            {
                requirementRecord.OnRequirementStatusChange += (record) =>
                {
                    onQuestRecordStatusChange?.Invoke(this);
                };
            }
        }
        public void RegisterObserverEvents()
        {
            requirementRecords.ForEach(x => x.RegisterObserverEvents());
        }
    }
}
