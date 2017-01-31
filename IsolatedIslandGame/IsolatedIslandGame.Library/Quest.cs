namespace IsolatedIslandGame.Library
{
    public abstract class Quest
    {
        public int QuestID { get; private set; }

        protected Quest(int questID)
        {
            QuestID = questID;
        }
    }
}
