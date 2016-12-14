using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class BlueprintRequirementRepository
    {
        public abstract Blueprint.ElementInfo Create(int blueprintID, Blueprint.ElementInfo requirement);
        public abstract void Delete(int blueprintID, Blueprint.ElementInfo requirement);
        public abstract List<Blueprint.ElementInfo> ListOfBlueprint(int blueprintID);
    }
}
