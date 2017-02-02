namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestReward
    {
        public abstract string Description { get; }

        public abstract bool GiveRewardCheck(Player player);
        public abstract void GiveReward(Player player);
    }
}
