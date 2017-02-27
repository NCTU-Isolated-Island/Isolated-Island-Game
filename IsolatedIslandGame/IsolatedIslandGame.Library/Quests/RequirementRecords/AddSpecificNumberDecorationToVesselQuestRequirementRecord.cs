using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class AddSpecificNumberDecorationToVesselQuestRequirementRecord : QuestRequirementRecord
    {
        private int addedDecorationNumber;
        public int AddedDecorationNumber
        {
            get { return addedDecorationNumber; }
            private set
            {
                addedDecorationNumber = value;
                QuestRecordFactory.Instance?.UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return AddedDecorationNumber >= (Requirement as AddSpecificNumberDecorationToVesselQuestRequirement).SpecificDecorationNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"裝飾數量： {AddedDecorationNumber}/{(Requirement as HaveSpecificNumberDecorationOnVesselQuestRequirement).SpecificDecorationNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public AddSpecificNumberDecorationToVesselQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int addedDecorationNumber) : base(questRequirementRecordID, requirement)
        {
            this.addedDecorationNumber = addedDecorationNumber;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.Vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
            {
                if (!IsSufficient && changeType == Protocol.DataChangeType.Add)
                {
                    AddedDecorationNumber = AddedDecorationNumber + 1;
                }
            };
        }
    }
}
