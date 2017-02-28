using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class Quest
    {
        public int QuestID { get; private set; }
        public QuestType QuestType { get; private set; }
        public string QuestName { get; private set; }
        public bool IsHidden { get; private set; }
        public string QuestDescription { get; private set; }

        private List<QuestRequirement> requirements = new List<QuestRequirement>();
        public IEnumerable<QuestRequirement> Requirements { get { return requirements.ToArray(); } }

        private List<QuestReward> rewards = new List<QuestReward>();
        public IEnumerable<QuestReward> Rewards { get { return rewards.ToArray(); } }

        public Quest(int questID, QuestType questType, string questName, bool isHidden, string questDescription, List<QuestRequirement> requirements, List<QuestReward> rewards)
        {
            QuestID = questID;
            QuestType = questType;
            QuestName = questName;
            IsHidden = isHidden;
            QuestDescription = questDescription;
            this.requirements = requirements;
            this.rewards = rewards;
        }

        public bool CreateRecord(int playerID, out QuestRecord record)
        {
            return QuestRecordFactory.Instance.CreateQuestRecord(playerID, this, out record);
        }
    }
}
