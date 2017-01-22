using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class TransactionRepository
    {
        public abstract bool Create(int requesterPlayerID, int accepterPlayerID, out Transaction transaction);
        public abstract void Save(Transaction transaction);
    }
}
