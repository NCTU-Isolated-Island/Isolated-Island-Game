using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int sendMaterialToIslandCount;
        public int SendMaterialToIslandCount
        {
            get { return sendMaterialToIslandCount; }
            private set
            {
                sendMaterialToIslandCount = value;
                QuestRecordFactory.Instance?.UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return SendMaterialToIslandCount >= (Requirement as SendMaterialToIslandSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"次數： {SendMaterialToIslandCount}/{(Requirement as SendMaterialToIslandSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int sendMaterialToIslandCount) : base(questRequirementRecordID, requirement)
        {
            this.sendMaterialToIslandCount = sendMaterialToIslandCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            Island.Instance.OnSendMaterial += (sendedPlayer, material) =>
            {
                if (!IsSufficient && sendedPlayer == player)
                {
                    SendMaterialToIslandCount = SendMaterialToIslandCount + 1;
                }
            };
        }
    }
}
