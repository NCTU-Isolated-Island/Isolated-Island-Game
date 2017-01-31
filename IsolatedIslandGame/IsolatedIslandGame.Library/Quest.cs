using IsolatedIslandGame.Protocol;
using System;

namespace IsolatedIslandGame.Library
{
    public abstract class Quest
    {
        public int QuestID { get; private set; }
        public abstract QuestType QuestType { get; }
        public string QuestName { get; private set; }
        public string Description { get; private set; }
        public bool IsTimeLimited { get; private set; }
        public DateTime DueTime { get; private set; }

        protected Quest(int questID, string questName, string description, bool isTimeLimited, DateTime dueTime)
        {
            QuestID = questID;
            QuestName = questName;
            Description = description;
            IsTimeLimited = isTimeLimited;
            DueTime = dueTime;
        }
    }
}
