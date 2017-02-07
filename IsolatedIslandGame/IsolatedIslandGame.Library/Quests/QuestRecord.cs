using System;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public class QuestRecord
    {
        [MessagePackMember(0)]
        public int QuestRecordID { get; private set; }
        [MessagePackMember(1)]
        public int PlayerID { get; private set; }
        [MessagePackMember(2)]
        public Quest Quest { get; private set; }
        [MessagePackMember(3)]
        [MessagePackRuntimeCollectionItemType]
        private List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();
        public IEnumerable<QuestRequirementRecord> RequirementRecords { get { return requirementRecords.ToArray(); } }
        [MessagePackMember(4)]
        private bool hasGottenReward;
        public bool HasGottenReward
        {
            get { return hasGottenReward; }
            private set
            {
                hasGottenReward = value;
                if (hasGottenReward == true)
                {
                    QuestRecordFactory.Instance?.MarkMarkQuestRecordHasGottenReward(QuestRecordID);
                }
            }
        }
        public bool IsFinished { get { return requirementRecords.TrueForAll(x => x.IsSufficient); } }

        private event Action<QuestRecord> onQuestRecordStatusChange;
        public event Action<QuestRecord> OnQuestRecordStatusChange { add { onQuestRecordStatusChange += value; } remove { onQuestRecordStatusChange -= value; } }

        private Action giveReward;

        [MessagePackDeserializationConstructor]
        public QuestRecord() { }
        public QuestRecord(int questRecordID, int playerID, Quest quest, List<QuestRequirementRecord> requirementRecords, bool hasGottenReward)
        {
            QuestRecordID = questRecordID;
            PlayerID = playerID;
            Quest = quest;
            this.requirementRecords = requirementRecords;
            HasGottenReward = hasGottenReward;
            foreach (var requirementRecord in requirementRecords)
            {
                requirementRecord.OnRequirementStatusChange += (record) =>
                {
                    onQuestRecordStatusChange?.Invoke(this);
                    if(IsFinished && !HasGottenReward)
                    {
                        giveReward?.Invoke();
                    }
                };
            }
        }
        public void RegisterObserverEvents(Player player)
        {
            requirementRecords.ForEach(x => x.RegisterObserverEvents(player));
            giveReward = () => 
            {
                HasGottenReward = true;
                foreach (var questReward in Quest.Rewards)
                {
                    if (questReward.GiveRewardCheck(player))
                    {
                        questReward.GiveReward(player);
                    }
                }
            };
            if (IsFinished && !HasGottenReward)
            {
                giveReward?.Invoke();
            }
        }
    }
}
