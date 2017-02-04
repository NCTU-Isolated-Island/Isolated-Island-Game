using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        public int QuestRequirementID { get; private set; }
        public abstract string Description { get; }
    }
}
