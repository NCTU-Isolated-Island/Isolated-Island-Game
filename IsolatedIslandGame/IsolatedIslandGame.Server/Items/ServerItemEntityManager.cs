using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Server.Items
{
    public class ServerItemEntityManager : ItemEntityManager
    {
        public ServerItemEntityManager()
        {
            foreach(var itemEntity in DatabaseService.RepositoryList.ItemEntityRepository.ListAll())
            {
                AddItemEntity(itemEntity);
            }

            OnItemEntityChange += ItemEntityChangeEvent;
        }

        private void ItemEntityChangeEvent(DataChangeType changeType, ItemEntity itemEntity)
        {
            SystemManager.Instance.EventManager.SyncDataResolver.SyncItemEntityChange(changeType, itemEntity);
            switch(changeType)
            {
                case DataChangeType.Remove:
                    DatabaseService.RepositoryList.ItemEntityRepository.Delete(itemEntity.ItemEntityID);
                    break;
            }
        }
    }
}
