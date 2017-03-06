using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberDecorationOnVesselQuestRequirementRecord : QuestRequirementRecord
    {
        private int decorationCount;
        public int DecorationCount
        {
            get { return decorationCount; }
            private set
            {
                decorationCount = value;
                QuestRecordFactory.Instance?.UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return DecorationCount >= (Requirement as HaveSpecificNumberDecorationOnVesselQuestRequirement).SpecificDecorationNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"裝飾數量： {DecorationCount}/{(Requirement as HaveSpecificNumberDecorationOnVesselQuestRequirement).SpecificDecorationNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int decorationCount) : base(questRequirementRecordID, requirement)
        {
            this.decorationCount = decorationCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (player.Vessel == null)
            {
                player.OnBindVessel += (vessel) => 
                {
                    if (!IsSufficient)
                    {
                        DecorationCount = vessel.DecorationCount;
                    }
                    vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                    {
                        if (!IsSufficient)
                        {
                            DecorationCount = vessel.DecorationCount;
                        }
                    };
                };
            }
            else
            {
                if (!IsSufficient)
                {
                    DecorationCount = player.Vessel.DecorationCount;
                }
                player.Vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
                {
                    if (!IsSufficient)
                    {
                        DecorationCount = player.Vessel.DecorationCount;
                    }
                };
            }
        }
    }
}
