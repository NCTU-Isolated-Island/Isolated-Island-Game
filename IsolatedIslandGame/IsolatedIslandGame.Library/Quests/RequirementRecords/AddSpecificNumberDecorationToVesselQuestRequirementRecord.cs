using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class AddSpecificNumberDecorationToVesselQuestRequirementRecord : QuestRequirementRecord
    {
        private int addedDecorationCount;
        public int AddedDecorationCount
        {
            get { return addedDecorationCount; }
            private set
            {
                addedDecorationCount = value;
                QuestRecordFactory.Instance?.UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return AddedDecorationCount >= (Requirement as AddSpecificNumberDecorationToVesselQuestRequirement).SpecificDecorationNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"裝飾數量： {AddedDecorationCount}/{(Requirement as AddSpecificNumberDecorationToVesselQuestRequirement).SpecificDecorationNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public AddSpecificNumberDecorationToVesselQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int addedDecorationCount) : base(questRequirementRecordID, requirement)
        {
            this.addedDecorationCount = addedDecorationCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (player.Vessel == null)
            {
                player.OnBindVessel += (vessel) => 
                {
                    vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                    {
                        if (!IsSufficient && changeType == Protocol.DataChangeType.Add)
                        {
                            AddedDecorationCount = AddedDecorationCount + 1;
                        }
                    };
                };
            }
            else
            {
                player.Vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                {
                    if (!IsSufficient && changeType == Protocol.DataChangeType.Add)
                    {
                        AddedDecorationCount = AddedDecorationCount + 1;
                    }
                };
            }
        }
    }
}
