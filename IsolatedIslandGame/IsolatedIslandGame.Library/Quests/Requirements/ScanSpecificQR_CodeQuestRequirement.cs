using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class ScanSpecificQR_CodeQuestRequirement : QuestRequirement
    {
        public string QR_CodeString { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.ScanSpecificQR_Code;
            }
        }
        public override string Description
        {
            get
            {
                return $"掃描指定QR Code";
            }
        }

        public ScanSpecificQR_CodeQuestRequirement(int questRequirementID, string qrCodeString) : base(questRequirementID)
        {
            QR_CodeString = qrCodeString;
        }
    }
}
