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

        public abstract Blueprint.ElementInfo CreateRequirement(int blueprintID, Blueprint.ElementInfo requirement);
        public abstract void DeleteRequirement(int blueprintID, Blueprint.ElementInfo requirement);
        public abstract List<Blueprint.ElementInfo> ListRequirementsOfBlueprint(int blueprintID);

        public abstract Blueprint.ElementInfo CreateProduct(int bluePrintID, Blueprint.ElementInfo product);
        public abstract void DeleteProduct(int blueprintID, Blueprint.ElementInfo product);
        public abstract List<Blueprint.ElementInfo> ListProductsOfBlueprint(int blueprintID);
    }
}
