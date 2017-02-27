using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberDecorationOnVesselQuestRequirementRecord : QuestRequirementRecord
    {
        private int decorationNumber;
        public int DecorationNumber
        {
            get { return decorationNumber; }
            private set
            {
                decorationNumber = value;
                QuestRecordFactory.Instance?.UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return DecorationNumber >= (Requirement as HaveSpecificNumberDecorationOnVesselQuestRequirement).SpecificDecorationNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"裝飾數量： {DecorationNumber}/{(Requirement as HaveSpecificNumberDecorationOnVesselQuestRequirement).SpecificDecorationNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int decorationNumber) : base(questRequirementRecordID, requirement)
        {
            this.decorationNumber = decorationNumber;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient)
            {
                DecorationNumber = player.Vessel.DecorationCount;
            }
            player.Vessel.OnDecorationChange += (changeType, vesselID, decoration) =>
            {
                if (!IsSufficient)
                {
                    DecorationNumber = player.Vessel.DecorationCount;
                }
            };
        }
    }
}
