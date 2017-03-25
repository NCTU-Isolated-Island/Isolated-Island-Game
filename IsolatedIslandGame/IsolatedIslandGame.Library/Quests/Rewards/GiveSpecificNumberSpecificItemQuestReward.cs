using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificItemQuestReward : QuestReward
    {
        public int ItemCount { get; private set; }
        public int ItemID { get; private set; }
        
        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificItem; } }
        public override string Description
        {
            get
            {
                Item item;
                if(ItemManager.Instance.FindItem(ItemID, out item))
                {
                    return $"給予{item.ItemName} x{ItemCount}";
                }
                else
                {
                    return $"給予未知的物品 x{ItemCount}";
                }
            }
        }

        public GiveSpecificNumberSpecificItemQuestReward(int questRewardID, int itemCount, int itemID) : base(questRewardID)
        {
            ItemCount = itemCount;
            ItemID = itemID;
        }

        public override void GiveReward(Player player)
        {
            Item item;
            if (ItemManager.Instance.FindItem(ItemID, out item))
            {
                player.Inventory.AddItem(item, ItemCount);
                player.User.EventManager.UserInform("提示", $"獲得了： {item.ItemName}x {ItemCount}");
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            return player.Inventory.AddItemCheck(ItemID, ItemCount);
        }
    }
}
