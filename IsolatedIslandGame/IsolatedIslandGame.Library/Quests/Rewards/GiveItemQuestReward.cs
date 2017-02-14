using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveItemQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int ItemID { get; private set; }
        [MessagePackMember(2)]
        public int ItemCount { get; private set; }
        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveItem; } }
        public override string Description
        {
            get
            {
                Item item;
                if(ItemManager.Instance.FindItem(ItemID, out item))
                {
                    return $"{item.ItemName} x{ItemCount}";
                }
                else
                {
                    return $"未知的物品 x{ItemCount}";
                }
            }
        }

        [MessagePackDeserializationConstructor]
        public GiveItemQuestReward() { }
        public GiveItemQuestReward(int questRewardID, int itemID, int itemCount) : base(questRewardID)
        {
            ItemID = itemID;
            ItemCount = itemCount;
        }

        public override void GiveReward(Player player)
        {
            Item item;
            if (ItemManager.Instance.FindItem(ItemID, out item))
            {
                player.Inventory.AddItem(item, ItemCount);
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            Item item;
            if (ItemManager.Instance.FindItem(ItemID, out item))
            {
                return player.Inventory.AddItemCheck(item.ItemID, ItemCount);
            }
            else
            {
                return false;
            }
        }
    }
}
