using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerRepository
    {
        public abstract bool Register(ulong facebookID);
        public abstract bool Contains(ulong facebookID, out int playerID);
        public abstract bool Read(int playerID, out Player player);
        public abstract void Update(Player player);
    }
}
