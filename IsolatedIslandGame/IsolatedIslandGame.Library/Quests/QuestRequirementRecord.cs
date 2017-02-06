using System;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirementRecord
    {
        [MessagePackMember(0)]
        public int QuestRequirementRecordID { get; private set; }
        [MessagePackMember(1)]
        [MessagePackRuntimeType]
        public QuestRequirement Requirement { get; private set; }
        public abstract string ProgressStatus { get; }
        public abstract bool IsSufficient { get; }
        public abstract event Action<QuestRequirementRecord> OnRequirementStatusChange;

        [MessagePackDeserializationConstructor]
        public QuestRequirementRecord() { }
        protected QuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement)
        {
            QuestRequirementRecordID = questRequirementRecordID;
            Requirement = requirement;
        }
        internal abstract void RegisterObserverEvents(Player player);
    }
}
