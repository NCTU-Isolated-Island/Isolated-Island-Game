using System;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Server
{
    public class ItemFactory : ItemManager
    {
        private event Action<Item> onItemUpdate;
        public override event Action<Item> OnItemUpdate { add { onItemUpdate += value; } remove { onItemUpdate -= value; } }

        public ItemFactory()
        {
            NextDrawMaterialTimeSpan = TimeSpan.FromSeconds(7200);
            var items = DatabaseService.RepositoryList.ItemRepository.ListAll();
            foreach(var item in items)
            {
                AddItem(item);
            }

            DateTime now = DateTime.Now;

            DatabaseService.RepositoryList.PlayerRepository.GlobalUpdateNextDrawMaterialTime(now);
            foreach (Player player in PlayerFactory.Instance.Players)
            {
                player.NextDrawMaterialTime = now;
            }

            NextDrawMaterialTime = DateTime.Today + TimeSpan.FromSeconds(((DateTime.Now - DateTime.Today).TotalSeconds / NextDrawMaterialTimeSpan.TotalSeconds) * NextDrawMaterialTimeSpan.TotalSeconds);
            Scheduler.Instance.AddTask(NextDrawMaterialTime + NextDrawMaterialTimeSpan, () => 
            {
                ResetDrawMaterialTime();
            });
        }

        public override void AddItem(Item item)
        {
            if(!ContainsItem(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
                onItemUpdate?.Invoke(item);
            }
        }

        public override bool FindItem(int itemID, out Item item)
        {
            if(ContainsItem(itemID))
            {
                item = itemDictionary[itemID];
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }

        public override bool SpecializeItemToMaterial(int itemID, out Material material)
        {
            if (ContainsItem(itemID))
            {
                Item item = itemDictionary[itemID];
                if(item is Material)
                {
                    material = item as Material;
                }
                else
                {
                    material = new Material(item.ItemID, item.ItemName, item.Description, 0, 0, Protocol.GroupType.No, 1);
                    itemDictionary[itemID] = material;
                    onItemUpdate?.Invoke(material);
                }
                return true;
            }
            else
            {
                material = null;
                return false;
            }
        }

        private void ResetDrawMaterialTime()
        {
            NextDrawMaterialTime = DateTime.Today + TimeSpan.FromSeconds(((DateTime.Now - DateTime.Today).TotalSeconds / NextDrawMaterialTimeSpan.TotalSeconds) * NextDrawMaterialTimeSpan.TotalSeconds);
            DatabaseService.RepositoryList.PlayerRepository.GlobalUpdateNextDrawMaterialTime(NextDrawMaterialTime);
            foreach (Player player in PlayerFactory.Instance.Players)
            {
                player.NextDrawMaterialTime = NextDrawMaterialTime;
            }
            Scheduler.Instance.AddTask(NextDrawMaterialTime + NextDrawMaterialTimeSpan, () =>
            {
                ResetDrawMaterialTime();
            });
        }
    }
}
