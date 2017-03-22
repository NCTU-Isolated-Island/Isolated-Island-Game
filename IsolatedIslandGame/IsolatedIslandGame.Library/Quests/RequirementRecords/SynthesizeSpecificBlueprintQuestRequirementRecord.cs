using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SynthesizeSpecificBlueprintQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasSynthesizedSpecificBlueprint;
        public bool HasSynthesizedSpecificBlueprint
        {
            get { return hasSynthesizedSpecificBlueprint; }
            private set
            {
                hasSynthesizedSpecificBlueprint = value;
                QuestRecordFactory.Instance?.UpdateSynthesizeSpecificBlueprintQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasSynthesizedSpecificBlueprint;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasSynthesizedSpecificBlueprint) ? "已合成" : "尚未合成" ;
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SynthesizeSpecificBlueprintQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasSynthesizedSpecificBlueprint) : base(questRequirementRecordID, requirement)
        {
            this.hasSynthesizedSpecificBlueprint = hasSynthesizedSpecificBlueprint;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnSynthesizeResultGenerated += (isSuccessful, resultBlueprint) =>
            {
                if (!IsSufficient && isSuccessful)
                {
                    if(resultBlueprint.BlueprintID == (Requirement as SynthesizeSpecificBlueprintQuestRequirement).SpecificBlueprintID)
                    {
                        HasSynthesizedSpecificBlueprint = true;
                    }
                }
            };
        }
    }
}
