using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class Quest
    {
        public int QuestID { get; private set; }
        public QuestType QuestType { get; private set; }
        public string QuestName { get; private set; }

        private List<QuestRequirement> requirements = new List<QuestRequirement>();
        public IEnumerable<QuestRequirement> Requirements { get { return requirements.ToArray(); } }

        private List<QuestReward> rewards = new List<QuestReward>();
        public IEnumerable<QuestReward> Rewards { get { return rewards.ToArray(); } }

        public string QuestDescription { get; private set; }
        public bool IsTimeLimited { get; private set; }
        public DateTime DueTime { get; private set; }

        public Quest(int questID, QuestType questType, string questName, List<QuestRequirement> requirements, List<QuestReward> rewards, string questDescription, bool isTimeLimited, DateTime dueTime)
        {
            QuestID = questID;
            QuestType = questType;
            QuestName = questName;
            this.requirements = requirements;
            this.rewards = rewards;
            QuestDescription = questDescription;
            IsTimeLimited = isTimeLimited;
            DueTime = dueTime;
        }
    }
}
