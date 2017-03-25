using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class RemoveSpecificNumberSpecificItemQuestReward : QuestReward
    {
        public int ItemCount { get; private set; }
        public int ItemID { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.RemoveSpecificNumberSpecificItem; } }
        public override string Description
        {
            get
            {
                Item item;
                if (ItemManager.Instance.FindItem(ItemID, out item))
                {
                    return $"消耗{item.ItemName} x{ItemCount}";
                }
                else
                {
                    return $"消耗未知的物品 x{ItemCount}";
                }
            }
        }

        public RemoveSpecificNumberSpecificItemQuestReward(int questRewardID, int itemCount, int itemID) : base(questRewardID)
        {
            ItemCount = itemCount;
            ItemID = itemID;
        }

        public override void GiveReward(Player player)
        {
            player.Inventory.RemoveItem(ItemID, ItemCount);
            player.User.EventManager.UserInform("提示", Description);
        }

        public override bool GiveRewardCheck(Player player)
        {
            return player.Inventory.RemoveItemCheck(ItemID, ItemCount);
        }
    }
}
