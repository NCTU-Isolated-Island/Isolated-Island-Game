namespace IsolatedIslandGame.Library.Items
{
    public class TransactionItemInfo
    {
        public Item Item { get; private set; }
        public int Count { get; set; }
        public int PositionIndex { get; set; }
        public TransactionItemInfo(Item item, int count, int positionIndex)
        {
            Item = item;
            Count = count;
            PositionIndex = positionIndex;

            ItemManager.Instance.OnItemUpdate += UpdateItem;
        }

        ~TransactionItemInfo()
        {
            ItemManager.Instance.OnItemUpdate -= UpdateItem;
        }

        private void UpdateItem(Item item)
        {
            if (item.ItemID == Item.ItemID)
            {
                Item = item;
            }
        }
    }
}
