using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public class MutiplayerSynthesizeQuest : Quest
    {
        public override QuestType QuestType { get { return QuestType.MutiplayerSynthesize; } }

        public MutiplayerSynthesizeQuest(int questID) : base(questID)
        {
        }
    }
}
