using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class StayInSpecificOceanQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasStayedInSpecificOcean;
        public bool HasStayedInSpecificOcean
        {
            get { return hasStayedInSpecificOcean; }
            private set
            {
                hasStayedInSpecificOcean = value;
                QuestRecordFactory.Instance?.UpdateStayInSpecificOceanQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasStayedInSpecificOcean;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasStayedInSpecificOcean) ? "已完成" : "尚未完成" ;
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public StayInSpecificOceanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasStayedInSpecificOcean) : base(questRequirementRecordID, requirement)
        {
            this.hasStayedInSpecificOcean = hasStayedInSpecificOcean;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (player.Vessel == null)
            {
                player.OnBindVessel += (vessel) => 
                {
                    if (!IsSufficient && vessel.LocatedOceanType == (Requirement as StayInSpecificOceanQuestRequirement).SpecificOceanType)
                    {
                        HasStayedInSpecificOcean = true;
                    }
                    vessel.OnVesselTransformUpdated += (vesselID, locationX, locationY, rotationEulerAngleY, locatedOceanType) =>
                    {
                        if (!IsSufficient && locatedOceanType == (Requirement as StayInSpecificOceanQuestRequirement).SpecificOceanType)
                        {
                            HasStayedInSpecificOcean = true;
                        }
                    };
                };
            }
            else
            {
                if (!IsSufficient && player.Vessel.LocatedOceanType == (Requirement as StayInSpecificOceanQuestRequirement).SpecificOceanType)
                {
                    HasStayedInSpecificOcean = true;
                }
                player.Vessel.OnVesselTransformUpdated += (vesselID, locationX, locationY, rotationEulerAngleY, locatedOceanType) =>
                {
                    if (!IsSufficient && locatedOceanType == (Requirement as StayInSpecificOceanQuestRequirement).SpecificOceanType)
                    {
                        HasStayedInSpecificOcean = true;
                    }
                };
            }
        }
    }
}
