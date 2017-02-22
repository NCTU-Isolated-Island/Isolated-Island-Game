using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests.Requirements;
using IsolatedIslandGame.Protocol;
using System.Linq;

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

        public bool FindCumulativeLoginQuest(int cumulativeLoginCount, out Quest quest)
        {
            if (Quests.Any(x => x.QuestType == QuestType.CumulativeLogin && x.Requirements.Any(y => y.QuestRequirementType == QuestRequirementType.CumulativeLogin)))
            {
                var cumulativeLoginQuests = Quests.Where(x => x.QuestType == QuestType.CumulativeLogin && x.Requirements.Any(y => y.QuestRequirementType == QuestRequirementType.CumulativeLogin));
                if(cumulativeLoginQuests.Any(x => x.Requirements.OfType<CumulativeLoginQuestRequirement>().Any(y => y.CumulativeLoginCount == cumulativeLoginCount)))
                {
                    quest = cumulativeLoginQuests.First(x => x.Requirements.OfType<CumulativeLoginQuestRequirement>().Any(y => y.CumulativeLoginCount == cumulativeLoginCount));
                    return true;
                }
                else
                {
                    quest = null;
                    return false;
                }
            }
            else
            {
                quest = null;
                return false;
            }
        }
    }
}
