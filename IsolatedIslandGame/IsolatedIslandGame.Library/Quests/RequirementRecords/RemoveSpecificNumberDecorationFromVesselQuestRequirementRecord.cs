using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class RemoveSpecificNumberDecorationFromVesselQuestRequirementRecord : QuestRequirementRecord
    {
        private int removedDecorationCount;
        public int RemovedDecorationCount
        {
            get { return removedDecorationCount; }
            private set
            {
                removedDecorationCount = value;
                QuestRecordFactory.Instance?.UpdateRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return RemovedDecorationCount >= (Requirement as RemoveSpecificNumberDecorationFromVesselQuestRequirement).SpecificDecorationNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"裝飾數量： {RemovedDecorationCount}/{(Requirement as RemoveSpecificNumberDecorationFromVesselQuestRequirement).SpecificDecorationNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public RemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int removedDecorationCount) : base(questRequirementRecordID, requirement)
        {
            this.removedDecorationCount = removedDecorationCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (player.Vessel == null)
            {
                player.OnBindVessel += (vessel) =>
                {
                    vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                    {
                        if (!IsSufficient && changeType == Protocol.DataChangeType.Remove)
                        {
                            RemovedDecorationCount = RemovedDecorationCount + 1;
                        }
                    };
                };
            }
            else
            {
                player.Vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                {
                    if (!IsSufficient && changeType == Protocol.DataChangeType.Remove)
                    {
                        RemovedDecorationCount = RemovedDecorationCount + 1;
                    }
                };
            }
        }
    }
}
