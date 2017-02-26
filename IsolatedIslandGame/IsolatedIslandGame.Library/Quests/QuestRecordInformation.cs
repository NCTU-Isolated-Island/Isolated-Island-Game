using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public struct QuestRecordInformation
    {
        public int questRecordID;
        public QuestType questType;
        public string questName;
        public string questDescription;
        public string requirementsDescription;
        public string rewardsDescription;
        public bool hasGottenReward;
        public bool isFinished;
    }
}
