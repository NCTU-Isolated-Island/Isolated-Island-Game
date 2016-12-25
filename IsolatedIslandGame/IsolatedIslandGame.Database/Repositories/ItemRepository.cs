using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class ItemRepository
    {
        public abstract bool Create(string itemName, string description, out Item item);
        public abstract bool CreateMaterial(string itemName, string description, int score, out Material material);
        public abstract bool Read(int itemID, out Item item);
        public abstract void Update(Item item);
        public abstract void Delete(int itemID);
        public abstract List<Item> ListAll();
    }
}
