using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public class Blueprint
    {
        public struct ElementInfo
        {
            public int itemID;
            public int itemCount;
            public int positionIndex;
        }

        public int BlueprintID { get; private set; }
        private ElementInfo[] requirements;
        public IEnumerable<ElementInfo> Requirements { get { return requirements.ToArray(); } }

        private ElementInfo[] products;
        public IEnumerable<ElementInfo> Products { get { return products.ToArray(); } }

        public Blueprint(int blueprintID, ElementInfo[] requirements, ElementInfo[] products)
        {
            BlueprintID = blueprintID;
            this.requirements = requirements;
            this.products = products;
        }

        public bool IsSufficientRequirements(List<ElementInfo> infoDictionary)
        {
            return requirements.All(requirement => infoDictionary.Contains(requirement));
        }
    }
}
