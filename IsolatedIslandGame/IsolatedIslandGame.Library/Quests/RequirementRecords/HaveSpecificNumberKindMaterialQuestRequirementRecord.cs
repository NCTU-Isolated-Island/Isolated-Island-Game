using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberKindMaterialQuestRequirementRecord : QuestRequirementRecord
    {
        private int materialKindNumber;
        public int MaterialKindNumber
        {
            get { return materialKindNumber; }
            private set
            {
                materialKindNumber = value;
                QuestRecordFactory.Instance?.UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return MaterialKindNumber >= (Requirement as HaveSpecificNumberKindMaterialQuestRequirement).SpecificKindNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"種類： {MaterialKindNumber}/{(Requirement as HaveSpecificNumberKindMaterialQuestRequirement).SpecificKindNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberKindMaterialQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int materialKindNumber) : base(questRequirementRecordID, requirement)
        {
            this.materialKindNumber = materialKindNumber;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient)
            {
                MaterialKindNumber = player.Inventory.DifferentMaterialCount;
            }
            player.Inventory.OnItemInfoChange += (changeType, info) =>
            {
                if (!IsSufficient)
                {
                    MaterialKindNumber = player.Inventory.DifferentMaterialCount;
                }
            };
        }
    }
}
