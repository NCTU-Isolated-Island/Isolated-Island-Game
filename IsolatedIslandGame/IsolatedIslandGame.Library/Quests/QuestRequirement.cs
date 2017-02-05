namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        public int QuestRequirementID { get; private set; }
        public abstract string Description { get; }
        public abstract QuestRequirementRecord CreateRequirementRecord(Player player);

        protected QuestRequirement(int questRequirementID)
        {
            QuestRequirementID = questRequirementID;
        }
    }
}
