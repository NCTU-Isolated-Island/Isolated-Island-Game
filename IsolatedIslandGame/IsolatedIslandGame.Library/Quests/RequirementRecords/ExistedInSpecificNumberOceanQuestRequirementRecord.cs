using IsolatedIslandGame.Library.Quests.Requirements;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class ExistedInSpecificNumberOceanQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<OceanType> existedOceanSet = new HashSet<OceanType>();
        public IEnumerable<OceanType> ExistedOceans { get { return existedOceanSet.ToArray(); } }
        public override bool IsSufficient
        {
            get
            {
                return existedOceanSet.Count >= (Requirement as ExistedInSpecificNumberOceanQuestRequirement).SpecificOceanNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"進度： {existedOceanSet.Count}/{(Requirement as ExistedInSpecificNumberOceanQuestRequirement).SpecificOceanNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public ExistedInSpecificNumberOceanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, HashSet<OceanType> existedOceanSet) : base(questRequirementRecordID, requirement)
        {
            this.existedOceanSet = existedOceanSet;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.Vessel.OnVesselTransformUpdated += (vesselID, locationX, locationY, rotationEulerAngleY, locatedOceanType) =>
            {
                if (!IsSufficient)
                {
                    lock (existedOceanSet)
                    {
                        if (!existedOceanSet.Contains(locatedOceanType))
                        {
                            if (existedOceanSet.Add(locatedOceanType) && (QuestRecordFactory.Instance == null || QuestRecordFactory.Instance.AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(QuestRequirementRecordID, locatedOceanType)))
                            {
                                onRequirementStatusChange?.Invoke(this);
                            }
                        }
                    }
                }
            };
        }
    }
}
