using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class QuestFactory
    {
        public static QuestFactory Instance { get; private set; }
        public static void Initial()
        {
            Instance = new QuestFactory();
        }

        protected Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

        private QuestFactory()
        {
            var quests = DatabaseService.RepositoryList.QuestRepository.ListAll();
            foreach (var quest in quests)
            {
                AddQuest(quest);
            }
        }
        public bool ContainsQuest(int questID)
        {
            return questDictionary.ContainsKey(questID);
        }
        public bool FindQuest(int questID, out Quest quest)
        {
            if (ContainsQuest(questID))
            {
                quest = questDictionary[questID];
                return true;
            }
            else
            {
                quest = null;
                return false;
            }
        }
        public void AddQuest(Quest quest)
        {
            if (!ContainsQuest(quest.QuestID))
            {
                questDictionary.Add(quest.QuestID, quest);
            }
        }
    }
}
