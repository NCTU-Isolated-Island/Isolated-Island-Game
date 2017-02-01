using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public class QR_CodeQuest : Quest
    {
        public override QuestType QuestType { get { return QuestType.QR_Code; } }

        public QR_CodeQuest(int questID, string questName, string description, bool isTimeLimited, DateTime dueTime) : base(questID, questName, description, isTimeLimited, dueTime)
        {
        }
    }
}
