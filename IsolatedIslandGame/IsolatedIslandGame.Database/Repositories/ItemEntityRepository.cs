using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class ItemEntityRepository
    {
        public abstract bool Create(int itemID, float positionX, float positionZ, out ItemEntity itemEntity);
        public abstract bool Read(int itemEntityID, out ItemEntity itemEntity);
        public abstract void Delete(int itemEntityID);
        public abstract List<ItemEntity> ListAll();
    }
}
