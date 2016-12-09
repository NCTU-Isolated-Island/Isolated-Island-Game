using System.Collections.Generic;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Library
{
    public abstract class VesselManager
    {
        public static VesselManager Instance { get; private set; }

        public static void Initial(VesselManager vesselManager)
        {
            Instance = vesselManager;
        }

        protected Dictionary<int, Vessel> vesselDictionary;
        protected Dictionary<int, Vessel> vesselDictionaryByOwnerPlayerID;
        public IEnumerable<Vessel> Vessels { get { return vesselDictionary.Values; } }
        public int VesselCount { get { return vesselDictionary.Count; } }

        public delegate void VesselChangeEventHandler(Vessel vessel, DataChangeType changeType);
        public abstract event VesselChangeEventHandler OnVesselChange;

        public abstract event Vessel.VesselTransformUpdatedEventHandler OnVesselTransformUpdated;
        public abstract event Vessel.DecorationChangeEventHandler OnVesselDecorationChange;

        protected VesselManager()
        {
            vesselDictionary = new Dictionary<int, Vessel>();
            vesselDictionaryByOwnerPlayerID = new Dictionary<int, Vessel>();
        }

        public bool ContainsVessel(int vesselID)
        {
            return vesselDictionary.ContainsKey(vesselID);
        }
        public bool ContainsVesselWithOwnerPlayerID(int ownerPlayerID)
        {
            return vesselDictionaryByOwnerPlayerID.ContainsKey(ownerPlayerID);
        }
        public abstract Vessel FindVessel(int vesselID);
        public abstract Vessel FindVesselByOwnerPlayerID(int ownerPlayerID);
        public abstract void AddVessel(Vessel vessel);
        public abstract bool RemoveVessel(int vesselID);
    }
}
