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

        public SendMessageQuest(int questID) : base(questID)
        {
        }
    }
}
