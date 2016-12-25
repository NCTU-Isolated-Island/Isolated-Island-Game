using IsolatedIslandGame.Library.Items;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class DecorationRepository
    {
        public abstract bool Create(int vesselID, int materialItemID, float positionX, float positionY, float positionZ, float eulerAngleX, float eulerAngleY, float eulerAngleZ, out Decoration decoration);
        public abstract bool Read(int decorationID, out Decoration decoration);
        public abstract void Update(Decoration decoration, int vesselID);
        public abstract void Delete(int decorationID);
        public abstract List<Decoration> ListOfVessel(int vesselID);
    }
}
