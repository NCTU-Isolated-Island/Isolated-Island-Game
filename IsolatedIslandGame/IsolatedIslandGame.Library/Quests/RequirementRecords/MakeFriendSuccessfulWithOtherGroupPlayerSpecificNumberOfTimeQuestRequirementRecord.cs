using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int successfulCount;
        public int SuccessfulCount
        {
            get { return successfulCount; }
            private set
            {
                successfulCount = value;
                QuestRecordFactory.Instance?.UpdateMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return SuccessfulCount >= (Requirement as MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"次數： {SuccessfulCount}/{(Requirement as MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int successfulCount) : base(questRequirementRecordID, requirement)
        {
            this.successfulCount = successfulCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnFriendInformationChange += (changeType, information) =>
            {
                PlayerInformation info;
                if (!IsSufficient && changeType == Protocol.DataChangeType.Update && information.isConfirmed &&
                    PlayerInformationManager.Instance.FindPlayerInformation(information.friendPlayerID, out info)
                    && info.groupType != (Requirement as MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirement).SpecificGroupType)
                {
                    SuccessfulCount = SuccessfulCount + 1;
                }
            };
        }
    }
}
