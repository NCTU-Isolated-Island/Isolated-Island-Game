using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class QuestFactory : QuestManager
    {
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
