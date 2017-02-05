namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRecordFactory
    {
        public static QuestRecordFactory Instance { get; private set; }
        public static void Initial(QuestRecordFactory factory)
        {
            Instance = factory;
        }

        public abstract bool CreateQuestRecord(Player player, Quest quest, out QuestRecord record);
        public abstract bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(Player player, QuestRequirement requirement, out QuestRequirementRecord record);
    }
}
