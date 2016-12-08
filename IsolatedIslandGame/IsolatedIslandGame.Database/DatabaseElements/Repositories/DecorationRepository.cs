using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.DatabaseElements.Repositories
{
    public abstract class DecorationRepository
    {
        public abstract Decoration Create(int vesselID, int materialItemID, float positionX, float positionY, float positionZ, float eulerAngleX, float eulerAngleY, float eulerAngleZ);
        public abstract Decoration Read(int decorationID);
        public abstract void Update(Decoration decoration, int vesselID);
        public abstract void Delete(int decorationID);
        public abstract List<Decoration> ListOfVessel(int vesselID);
    }
}
