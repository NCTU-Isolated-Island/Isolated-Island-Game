using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library
{
    public class Quest
    {
        [MessagePackMember(0)]
        public int QuestID { get; private set; }
        [MessagePackMember(1)]
        public QuestType QuestType { get; private set; }
        [MessagePackMember(2)]
        public string QuestName { get; private set; }

        [MessagePackMember(3)]
        [MessagePackRuntimeCollectionItemType]
        private List<QuestRequirement> requirements = new List<QuestRequirement>();
        public IEnumerable<QuestRequirement> Requirements { get { return requirements.ToArray(); } }

        [MessagePackMember(4)]
        [MessagePackRuntimeCollectionItemType]
        private List<QuestReward> rewards = new List<QuestReward>();
        public IEnumerable<QuestReward> Rewards { get { return rewards.ToArray(); } }

        [MessagePackMember(5)]
        public string QuestDescription { get; private set; }

        [MessagePackDeserializationConstructor]
        public Quest() { }
        public Quest(int questID, QuestType questType, string questName, List<QuestRequirement> requirements, List<QuestReward> rewards, string questDescription)
        {
            QuestID = questID;
            QuestType = questType;
            QuestName = questName;
            this.requirements = requirements;
            this.rewards = rewards;
            QuestDescription = questDescription;
        }

        public bool CreateRecord(int playerID, out QuestRecord record)
        {
            return QuestRecordFactory.Instance.CreateQuestRecord(playerID, this, out record);
        }
    }
}
