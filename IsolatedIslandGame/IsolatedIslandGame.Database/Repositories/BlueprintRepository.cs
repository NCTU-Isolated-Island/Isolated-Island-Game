using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class BlueprintRepository
    {
        protected struct BlueprintInfo
        {
            public int blueprintID;
            public bool isOrderless;
            public bool isBlueprintRequired;
        }

        public abstract Blueprint Create(bool isOrderless, bool isBlueprintRequired, Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products);
        public abstract void Delete(int blueprintID);
        public abstract List<Blueprint> ListAll();
    }
}
