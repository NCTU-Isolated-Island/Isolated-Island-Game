using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class DrawMaterialQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasDrawMaterial;
        public bool HasDrawMaterial
        {
            get { return hasDrawMaterial; }
            private set
            {
                hasDrawMaterial = value;
                QuestRecordFactory.Instance?.UpdateDrawMaterialQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasDrawMaterial;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasDrawMaterial) ? "已完成" : "尚未完成";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public DrawMaterialQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasDrawMaterial) : base(questRequirementRecordID, requirement)
        {
            this.hasDrawMaterial = hasDrawMaterial;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnDrawMaterial += () =>
            {
                if (!IsSufficient)
                {
                    HasDrawMaterial = true;
                }
            };
        }
    }
}
