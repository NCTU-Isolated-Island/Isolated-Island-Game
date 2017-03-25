using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class StillNotOpenedQuestRequirement : QuestRequirement
    {

        public override string Description
        {
            get
            {
                return "尚未開放";
            }
        }

        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.StillNotOpened;
            }
        }

        public StillNotOpenedQuestRequirement(int questRequirementID) : base(questRequirementID)
        {
        }
    }
}
