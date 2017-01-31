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

        public CloseDealQuest(int questID) : base(questID)
        {
        }
    }
}
