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

        public override event BlueprintChangeEventHandler OnBlueprintChange;

        public override void AddBlueprint(Blueprint blueprint)
        {
            if (!ContainsBlueprint(blueprint.BlueprintID))
            {
                blueprintDictionary.Add(blueprint.BlueprintID, blueprint);
            }
        }

        public override bool FindBlueprint(int blueprintID, out Blueprint blueprint)
        {
            if (ContainsBlueprint(blueprintID))
            {
                blueprint = blueprintDictionary[blueprintID];
                return true;
            }
            else
            {
                blueprint = null;
                return false;
            }
        }

        public override bool RemoveBlueprint(int blueprintID)
        {
            if (ContainsBlueprint(blueprintID))
            {
                blueprintDictionary.Remove(blueprintID);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
