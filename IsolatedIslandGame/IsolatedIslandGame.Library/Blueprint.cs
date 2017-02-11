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
        public bool IsOrderless { get; private set; }
        public bool IsBlueprintRequired { get; private set; }
        private ElementInfo[] requirements;
        public IEnumerable<ElementInfo> Requirements { get { return requirements.ToArray(); } }

        private ElementInfo[] products;
        public IEnumerable<ElementInfo> Products { get { return products.ToArray(); } }

        public Blueprint(int blueprintID, bool isOrderless, bool isBlueprintRequired, ElementInfo[] requirements, ElementInfo[] products)
        {
            BlueprintID = blueprintID;
            IsOrderless = isOrderless;
            IsBlueprintRequired = isBlueprintRequired;
            this.requirements = requirements;
            this.products = products;
        }

        public bool IsSufficientRequirements(List<ElementInfo> infos)
        {
            infos = new List<ElementInfo>(infos);
            if (IsOrderless)
            {
                foreach (ElementInfo require in requirements)
                {
                    if(infos.Any(x => x.itemID == require.itemID && x.itemCount == require.itemCount))
                    {
                        infos.Remove(infos.First(x => x.itemID == require.itemID && x.itemCount == require.itemCount));
                    }
                    else
                    {
                        return false;
                    }
                }
                return infos.Count() == 0;
            }
            else
            {
                return requirements.All(requirement => infos.Contains(requirement));
            }
        }
    }
}
