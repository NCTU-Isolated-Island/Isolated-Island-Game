using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class VesselRepository
    {
        public abstract Vessel Create(int ownerPlayerID, string name);
        public abstract Vessel Read(int vesselID);
        public abstract Vessel ReadByOwnerPlayerID(int ownerPlayerID);
        public abstract void Update(Vessel vessel);
        public abstract void Delete(int vesselID);
    }
}
