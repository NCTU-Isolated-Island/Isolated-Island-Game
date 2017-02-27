using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.Quests.Requirements;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord : QuestRequirementRecord
    {
        private int materialCount;
        public int MaterialCount
        {
            get { return materialCount; }
            private set
            {
                materialCount = value;
                QuestRecordFactory.Instance?.UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return MaterialCount >= (Requirement as CollectSpecificNumberBelongingGroupMaterialQuestRequirement).SpecificMaterialNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"素材數量： {MaterialCount}/{(Requirement as CollectSpecificNumberBelongingGroupMaterialQuestRequirement).SpecificMaterialNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int materialCount) : base(questRequirementRecordID, requirement)
        {
            this.materialCount = materialCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient)
            {
                MaterialCount = player.Inventory.ItemInfos.Select(x => x.Item).OfType<Material>().Count(x => x.GroupType == player.GroupType);
            }
            player.Inventory.OnItemCountChange += (itemID, countDelta) =>
            {
                if (!IsSufficient)
                {
                    MaterialCount = player.Inventory.ItemInfos.Select(x => x.Item).OfType<Material>().Count(x => x.GroupType == player.GroupType);
                }
            };
        }
    }
}
