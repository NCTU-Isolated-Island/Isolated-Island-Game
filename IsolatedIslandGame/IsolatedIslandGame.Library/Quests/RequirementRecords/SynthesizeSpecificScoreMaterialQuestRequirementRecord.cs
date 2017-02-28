using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SynthesizeSpecificScoreMaterialQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasSynthesizedSpecificScoreMaterial;
        public bool HasSynthesizedSpecificScoreMaterial
        {
            get { return hasSynthesizedSpecificScoreMaterial; }
            private set
            {
                hasSynthesizedSpecificScoreMaterial = value;
                QuestRecordFactory.Instance?.UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasSynthesizedSpecificScoreMaterial;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasSynthesizedSpecificScoreMaterial) ? "已合成" : "尚未合成" ;
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SynthesizeSpecificScoreMaterialQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasSynthesizedSpecificScoreMaterial) : base(questRequirementRecordID, requirement)
        {
            this.hasSynthesizedSpecificScoreMaterial = hasSynthesizedSpecificScoreMaterial;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnSynthesizeResultGenerated += (isSuccessful, resultBlueprint) =>
            {
                if (!IsSufficient && isSuccessful)
                {
                    foreach(var product in resultBlueprint.Products)
                    {
                        Item item;
                        if(ItemManager.Instance.FindItem(product.itemID, out item) && 
                            item is Material && 
                            (item as Material).Score == (Requirement as SynthesizeSpecificScoreMaterialQuestRequirement).SpecificMaterialScore)
                        {
                            HasSynthesizedSpecificScoreMaterial = true;
                            break;
                        }
                    }
                }
            };
        }
    }
}
