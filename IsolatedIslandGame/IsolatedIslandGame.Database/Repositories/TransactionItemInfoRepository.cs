using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class TransactionItemInfoRepository
    {
        public abstract void Save(int transactionID, int playerID, TransactionItemInfo info);
    }
}
