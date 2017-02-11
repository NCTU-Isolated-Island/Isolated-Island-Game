using IsolatedIslandGame.Library;
using System;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerRepository
    {
        public abstract bool Register(ulong facebookID);
        public abstract bool Contains(ulong facebookID, out int playerID);
        public abstract bool Read(int playerID, out Player player);
        public abstract void Update(Player player);
        public abstract bool ReadPlayerInformation(int playerID, out PlayerInformation playerInformation);
        public abstract void GlobalUpdateNextDrawMaterialTime(DateTime nextDrawMaterialTime);
    }
}
