using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public class CloseDealQuest : Quest
    {
        public override QuestType QuestType { get { return QuestType.CloseDeal; } }
        public int DealCount { get; private set; }

        public CloseDealQuest(int questID, string questName, string description, bool isTimeLimited, DateTime dueTime) : base(questID, questName, description, isTimeLimited, dueTime)
        {
        }
    }
}
