using System;

namespace IsolatedIslandGame.Library.UnitTest
{
    class TestVesselManager : VesselManager
    {
        public override event VesselChangeEventHandler OnVesselChange;
        public override event Vessel.DecorationChangeEventHandler OnVesselDecorationChange;
        public override event Vessel.VesselTransformUpdatedEventHandler OnVesselTransformUpdated;

        public override void AddVessel(Vessel vessel)
        {
            if (!ContainsVessel(vessel.VesselID))
            {
                vesselDictionary.Add(vessel.VesselID, vessel);
                vesselDictionaryByOwnerPlayerID.Add(vessel.OwnerPlayerID, vessel);
            }
        }

        public override bool FindVessel(int vesselID, out Vessel vessel)
        {
            throw new NotImplementedException();
        }

        public override bool FindVesselByOwnerPlayerID(int ownerPlayerID, out Vessel vessel)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveVessel(int vesselID)
        {
            throw new NotImplementedException();
        }
    }
}
