namespace IsolatedIslandGame.Library.Items
{
    public class ItemEntityGeneratingFactor
    {
        public int ItemEntityGeneratingFactorID { get; private set; }
        public int GeneratingItemID { get; private set; }
        public int GeneratingWeight { get; private set; }

        public ItemEntityGeneratingFactor(int itemEntityGeneratingFactorID, int generatingItemID, int generatingWeight)
        {
            ItemEntityGeneratingFactorID = itemEntityGeneratingFactorID;
            GeneratingItemID = generatingItemID;
            GeneratingWeight = generatingWeight;
        }
    }
}
