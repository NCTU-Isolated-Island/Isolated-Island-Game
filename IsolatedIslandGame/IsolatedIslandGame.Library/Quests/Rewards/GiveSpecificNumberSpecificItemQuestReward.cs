using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificItemQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int ItemCount { get; private set; }
        [MessagePackMember(2)]
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

        [MessagePackDeserializationConstructor]
        public GiveSpecificNumberSpecificItemQuestReward() { }
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
