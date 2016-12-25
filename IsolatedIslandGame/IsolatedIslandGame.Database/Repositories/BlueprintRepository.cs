using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class BlueprintRepository
    {
        public abstract Blueprint Create(Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products);
        public abstract void Delete(int blueprintID);
        public abstract List<Blueprint> ListAll();
    }
}
