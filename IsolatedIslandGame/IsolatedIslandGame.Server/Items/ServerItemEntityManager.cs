using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;

namespace IsolatedIslandGame.Server.Items
{
    public class ServerItemEntityManager : ItemEntityManager
    {
        public ServerItemEntityManager()
        {
            OnItemEntityChange += ItemEntityChangeEvent;
            foreach (var itemEntity in DatabaseService.RepositoryList.ItemEntityRepository.ListAll())
            {
                AddItemEntity(itemEntity);
            }
        }

        private void ItemEntityChangeEvent(DataChangeType changeType, ItemEntity itemEntity)
        {
            SystemManager.Instance.EventManager.SyncDataResolver.SyncItemEntityChange(changeType, itemEntity);
            switch(changeType)
            {
                case DataChangeType.Add:
                    {
                        Scheduler.Instance.AddTask(DateTime.Now + TimeSpan.FromHours(1), () => RemoveItemEntity(itemEntity.ItemEntityID));
                    }
                    break;
                case DataChangeType.Remove:
                    DatabaseService.RepositoryList.ItemEntityRepository.Delete(itemEntity.ItemEntityID);
                    break;
            }
        }
    }
}
