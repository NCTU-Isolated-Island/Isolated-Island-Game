using System;
using System.Collections.Generic;
using System.Text;

namespace IsolatedIslandGame.Library.Quests
{
    public class QuestRecord
    {
        public int QuestRecordID { get; private set; }
        public int PlayerID { get; private set; }
        public Quest Quest { get; private set; }
        private List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();
        public IEnumerable<QuestRequirementRecord> RequirementRecords { get { return requirementRecords.ToArray(); } }
        private bool hasGottenReward;
        public bool HasGottenReward
        {
            get { return hasGottenReward; }
            private set
            {
                hasGottenReward = value;
                if (hasGottenReward == true)
                {
                    QuestRecordFactory.Instance?.MarkQuestRecordHasGottenReward(QuestRecordID);
                }
                onQuestRecordStatusChange?.Invoke(this);
            }
        }
        public bool IsFinished { get { return requirementRecords.TrueForAll(x => x.IsSufficient); } }

        private event Action<QuestRecord> onQuestRecordStatusChange;
        public event Action<QuestRecord> OnQuestRecordStatusChange { add { onQuestRecordStatusChange += value; } remove { onQuestRecordStatusChange -= value; } }

        private Action giveReward;

        public QuestRecordInformation QuestRecordInformation
        {
            get
            {
                QuestRecordInformation information = new QuestRecordInformation();
                information.questRecordID = QuestRecordID;
                information.questType = Quest.QuestType;
                information.questName = Quest.QuestName;
                information.questDescription = Quest.QuestDescription;

                StringBuilder requirementDescriptionBuilder = new StringBuilder();
                foreach (var requirementRecord in RequirementRecords)
                {
                    requirementDescriptionBuilder.AppendLine($"{requirementRecord.Requirement.Description}\t\t{requirementRecord.ProgressStatus}");
                }
                information.requirementsDescription = requirementDescriptionBuilder.ToString();
                StringBuilder rewardDescriptionBuilder = new StringBuilder();
                foreach(var reward in Quest.Rewards)
                {
                    rewardDescriptionBuilder.AppendLine(reward.Description);
                }
                information.rewardsDescription = rewardDescriptionBuilder.ToString();
                information.hasGottenReward = HasGottenReward;
                information.isFinished = IsFinished;

                return information;
            }
        }

        public QuestRecord(int questRecordID, int playerID, Quest quest, List<QuestRequirementRecord> requirementRecords, bool hasGottenReward)
        {
            QuestRecordID = questRecordID;
            PlayerID = playerID;
            Quest = quest;
            this.requirementRecords = requirementRecords;
            this.hasGottenReward = hasGottenReward;
            foreach (var requirementRecord in requirementRecords)
            {
                requirementRecord.OnRequirementStatusChange += (record) =>
                {
                    onQuestRecordStatusChange?.Invoke(this);
                };
            }
        }
        public void RegisterObserverEvents(Player player)
        {
            if(!IsFinished)
            {
                requirementRecords.ForEach(x => x.RegisterObserverEvents(player));
            }
            giveReward = () => 
            {
                bool canGiveReward = true;
                foreach (var questReward in Quest.Rewards)
                {
                    if (!questReward.GiveRewardCheck(player))
                    {
                        canGiveReward = false;
                        break;
                    }
                }
                if(canGiveReward)
                {
                    foreach (var questReward in Quest.Rewards)
                    {
                        questReward.GiveReward(player);
                    }
                    HasGottenReward = true;
                }
            };
        }
        public void GiveReward()
        {
            if (IsFinished && !HasGottenReward)
            {
                giveReward?.Invoke();
            }
        }
    }
}
