using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class VesselRepository
    {
        public abstract bool Create(Player player, out Vessel vessel);
        public abstract bool Read(int vesselID, out Vessel vessel);
        public abstract bool ReadByOwnerPlayerID(int ownerPlayerID, out Vessel vessel);
        public abstract void Update(Vessel vessel);
        public abstract void Delete(int vesselID);
    }
}
