using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageQuest : Quest
    {
        public override QuestType QuestType { get { return QuestType.SendMessage; } }

        public SendMessageQuest(int questID, string questName, string description, bool isTimeLimited, DateTime dueTime) : base(questID, questName, description, isTimeLimited, dueTime)
        {
        }
    }
}
