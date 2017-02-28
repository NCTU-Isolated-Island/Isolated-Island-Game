using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class GetSpecificItemQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasGottenSpecificItem;
        public bool HasGottenSpecificItem
        {
            get { return hasGottenSpecificItem; }
            private set
            {
                hasGottenSpecificItem = value;
                QuestRecordFactory.Instance?.UpdateGetSpecificItemQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasGottenSpecificItem;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasGottenSpecificItem) ? "已取得" : "尚未取得";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public GetSpecificItemQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasGottenSpecificItem) : base(questRequirementRecordID, requirement)
        {
            this.hasGottenSpecificItem = hasGottenSpecificItem;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient && player.Inventory.ItemCount((Requirement as GetSpecificItemQuestRequirement).SpecificItemID) > 0)
            {
                HasGottenSpecificItem = true;
            }
            player.Inventory.OnItemCountChange += (itemID, countDelta) =>
            {
                if (!IsSufficient && player.Inventory.ItemCount((Requirement as GetSpecificItemQuestRequirement).SpecificItemID) > 0)
                {
                    HasGottenSpecificItem = true;
                }
            };
        }
    }
}
