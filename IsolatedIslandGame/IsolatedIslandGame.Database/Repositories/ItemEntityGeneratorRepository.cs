using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class ItemEntityGeneratorRepository
    {
        public abstract List<ItemEntityGenerator> ListAllItemEntityGenerators();
        public abstract List<ItemEntityGeneratingFactor> ListItemEntityGeneratingFactorsOfGenerator(int itemEntityGeneratorID);
    }
}
