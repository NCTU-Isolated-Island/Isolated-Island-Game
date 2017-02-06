using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class BlueprintFactory : BlueprintManager
    {
        public BlueprintFactory()
        {
            var blueprints = DatabaseService.RepositoryList.BlueprintRepository.ListAll();
            foreach (var blueprint in blueprints)
            {
                AddBlueprint(blueprint);
            }
        }
    }
}
