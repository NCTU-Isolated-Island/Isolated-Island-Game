using IsolatedIslandGame.Library.Quests.Requirements;
using MsgPack.Serialization;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class ScanQR_CodeQuestRequirementRecord : QuestRequirementRecord
    {
        [MessagePackMember(3)]
        private bool hasScannedCorrectQR_Code;
        public bool HasScannedCorrectQR_Code
        {
            get { return hasScannedCorrectQR_Code; }
            private set
            {
                hasScannedCorrectQR_Code = value;
                if (hasScannedCorrectQR_Code == true)
                {
                    QuestRecordFactory.Instance?.MarkScanQR_CodeQuestRequirementRecordHasScannedCorrectQR_Code(QuestRequirementRecordID);
                }
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
                return (IsSufficient) ? "掃描成功" : "尚未掃描對應的QR Code";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        [MessagePackDeserializationConstructor]
        public ScanQR_CodeQuestRequirementRecord() { }
        public ScanQR_CodeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasScannedCorrectScanQR_Code) : base(questRequirementRecordID, requirement)
        {
            this.hasScannedCorrectQR_Code = hasScannedCorrectScanQR_Code;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnScanQR_Code += (qrCodeString) =>
            {
                if (!IsSufficient && qrCodeString == (Requirement as ScanQR_CodeQuestRequirement).QR_CodeString)
                {
                    HasScannedCorrectQR_Code = true;
                    onRequirementStatusChange?.Invoke(this);
                }
            };
        }
    }
}
