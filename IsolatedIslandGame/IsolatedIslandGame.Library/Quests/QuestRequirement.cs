namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        public int QuestRequirementID { get; private set; }
        public abstract string Description { get; }
        public abstract bool CreateRequirementRecord(Player player, out QuestRequirementRecord record);

        protected QuestRequirement(int questRequirementID)
        {
            QuestRequirementID = questRequirementID;
        }
    }
}
