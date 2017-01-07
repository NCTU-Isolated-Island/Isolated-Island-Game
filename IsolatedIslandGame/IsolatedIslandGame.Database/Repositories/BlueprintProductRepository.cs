using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class BlueprintProductRepository
    {
        public abstract Blueprint.ElementInfo Create(int bluePrintID, Blueprint.ElementInfo product);
        public abstract void Delete(int blueprintID, Blueprint.ElementInfo product);
        public abstract List<Blueprint.ElementInfo> ListOfBlueprint(int blueprintID);
    }
}
