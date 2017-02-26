using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Server
{
    public class QuestFactory : QuestManager
    {
        private List<Quest> initialQuests = new List<Quest>();
        public IEnumerable<Quest> InitialQuests { get { return initialQuests.ToArray(); } }

        public QuestFactory()
        {
            var quests = DatabaseService.RepositoryList.QuestRepository.ListAll();
            foreach (var quest in quests)
            {
                AddQuest(quest);
            }
        }
    }
}
