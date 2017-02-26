using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class ItemRepository
    {
        public abstract bool Read(int itemID, out Item item);
        public abstract List<Item> ListAll();
    }
}
