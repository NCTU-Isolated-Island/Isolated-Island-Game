using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class ItemRepository
    {
        public abstract Item Create(string itemName, string description);
        public abstract Material CreateMaterial(string itemName, string description);
        public abstract Item Read(int itemID);
        public abstract void Update(Item item);
        public abstract void Delete(int itemID);
        public abstract List<Item> ListAll();
    }
}
