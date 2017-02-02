namespace IsolatedIslandGame.Library.Quests
{
    public class GiveItemQuestReward : QuestReward
    {
        public Item Item { get; private set; }
        public int ItemCount { get; private set; }
        public override string Description
        {
            get
            {
                return $"{Item.ItemName} x{ItemCount}";
            }
        }

        public override void GiveReward(Player player)
        {
            player.Inventory.AddItem(Item, ItemCount);
        }

        public override bool GiveRewardCheck(Player player)
        {
            return player.Inventory.AddItemCheck(Item.ItemID, ItemCount);
        }
    }
}
