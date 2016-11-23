using IsolatedIslandGame.Database.DatabaseFormatData;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class PlayerRepository
    {
        public abstract bool Register(ulong facebookID);
        public abstract bool Contains(ulong facebookID, out int playerID);
        public abstract PlayerData Find(int playerID);
        public abstract void Save(Player player);
    }
}
