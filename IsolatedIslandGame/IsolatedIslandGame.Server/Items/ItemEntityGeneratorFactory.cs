using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server.Items
{
    public class ItemEntityGeneratorFactory
    {
        public static ItemEntityGeneratorFactory Instance { get; private set; }
        public static void Initial(ItemEntityGeneratorFactory factory)
        {
            Instance = factory;
        }

        private Dictionary<int, ItemEntityGenerator> itemEntityGeneratorDictionary = new Dictionary<int, ItemEntityGenerator>();

        public ItemEntityGeneratorFactory()
        {
            foreach(var generator in DatabaseService.RepositoryList.ItemEntityGeneratorRepository.ListAllItemEntityGenerators())
            {
                itemEntityGeneratorDictionary.Add(generator.ItemEntityGeneratorID, generator);
                generator.GenerateItemEntityRoutine();
            }
        }
    }
}
