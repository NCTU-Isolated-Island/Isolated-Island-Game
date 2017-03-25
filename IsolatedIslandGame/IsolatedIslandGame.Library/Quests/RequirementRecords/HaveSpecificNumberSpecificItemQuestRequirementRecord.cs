using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberSpecificItemQuestRequirementRecord : QuestRequirementRecord
    {
        private Inventory inventory;
        public override bool IsSufficient
        {
            get
            {
                if(inventory != null)
                {
                    return inventory.ItemCount((Requirement as HaveSpecificNumberSpecificItemQuestRequirement).SpecificItemID) >= (Requirement as HaveSpecificNumberSpecificItemQuestRequirement).SpecificNumber;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string ProgressStatus
        {
            get
            {
                if (inventory != null)
                {
                    return $"數量： {inventory.ItemCount((Requirement as HaveSpecificNumberSpecificItemQuestRequirement).SpecificItemID)}/{(Requirement as HaveSpecificNumberSpecificItemQuestRequirement).SpecificNumber}";
                }
                else
                {
                    return $"數量： 0";
                }
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberSpecificItemQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement) : base(questRequirementRecordID, requirement)
        {

        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (player.Inventory == null)
            {
                player.OnBindInventory += (inventory) =>
                {
                    this.inventory = inventory;
                    inventory.OnItemCountChange += (itemID, countDelta) =>
                    {
                        onRequirementStatusChange?.Invoke(this);
                    };
                };
            }
            else
            {
                this.inventory = player.Inventory;
                player.Inventory.OnItemCountChange += (itemID, countDelta) =>
                {
                    onRequirementStatusChange?.Invoke(this);
                };
            }
        }
    }
}
