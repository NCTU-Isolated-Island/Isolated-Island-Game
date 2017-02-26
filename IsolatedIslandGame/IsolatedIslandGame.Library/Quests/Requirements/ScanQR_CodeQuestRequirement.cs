using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class ScanQR_CodeQuestRequirement : QuestRequirement
    {
        public string QR_CodeString { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.ScanQR_Code;
            }
        }
        public override string Description
        {
            get
            {
                return $"掃描QR Code";
            }
        }

        public ScanQR_CodeQuestRequirement(int questRequirementID, string qrCodeString) : base(questRequirementID)
        {
            QR_CodeString = qrCodeString;
        }
    }
}
