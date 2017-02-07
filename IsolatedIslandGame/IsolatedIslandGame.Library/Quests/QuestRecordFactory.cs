namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRecordFactory
    {
        public static QuestRecordFactory Instance { get; private set; }
        public static void Initial(QuestRecordFactory factory)
        {
            Instance = factory;
        }

        public abstract bool CreateQuestRecord(int playerID, Quest quest, out QuestRecord record);
        public abstract bool CreateQuestRequirementRecord(int questRecordID, int playerID, QuestRequirement requirement, out QuestRequirementRecord record);
        public abstract bool MarkMarkQuestRecordHasGottenReward(int questRecordID);

        public abstract bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
        public abstract bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
    }
}
