using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class ScanSpecificQR_CodeQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasScannedCorrectQR_Code;
        public bool HasScannedCorrectQR_Code
        {
            get { return hasScannedCorrectQR_Code; }
            private set
            {
                hasScannedCorrectQR_Code = value;
                QuestRecordFactory.Instance?.UpdateScanSpecificQR_CodeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasScannedCorrectQR_Code;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasScannedCorrectQR_Code) ? "掃描成功" : "尚未掃描到對應的QR Code";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public ScanSpecificQR_CodeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasScannedCorrectQR_Code) : base(questRequirementRecordID, requirement)
        {
            this.hasScannedCorrectQR_Code = hasScannedCorrectQR_Code;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnScanQR_Code += (QR_CodeString) =>
            {
                if (!IsSufficient)
                {
                    HasScannedCorrectQR_Code = (QR_CodeString == (Requirement as ScanSpecificQR_CodeQuestRequirement).QR_CodeString);
                }
            };
        }
    }
}
