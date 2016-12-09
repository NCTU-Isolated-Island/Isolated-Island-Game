using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Server
{
    public class ServerVesselManager : VesselManager
    {
        public override event Vessel.DecorationChangeEventHandler OnVesselDecorationChange;
        public override event Vessel.VesselTransformUpdatedEventHandler OnVesselTransformUpdated;
        private event VesselChangeEventHandler onVesselChange;
        public override event VesselChangeEventHandler OnVesselChange { add { onVesselChange += value; } remove { onVesselChange -= value; } }

        public ServerVesselManager()
        {
            onVesselChange += SystemManager.Instance.EventManager.SyncDataResolver.SyncVesselChange;
        }

        public override void AddVessel(Vessel vessel)
        {
            if (!ContainsVessel(vessel.VesselID))
            {
                vesselDictionary.Add(vessel.VesselID, vessel);
                vesselDictionaryByOwnerPlayerID.Add(vessel.OwnerPlayerID, vessel);
                onVesselChange(vessel, DataChangeType.Add);
            }
        }

        public override Vessel FindVessel(int vesselID)
        {
            if (ContainsVessel(vesselID))
            {
                return vesselDictionary[vesselID];
            }
            else
            {
                return null;
            }
        }

        public override Vessel FindVesselByOwnerPlayerID(int ownerPlayerID)
        {
            if(ContainsVesselWithOwnerPlayerID(ownerPlayerID))
            {
                return vesselDictionaryByOwnerPlayerID[ownerPlayerID];
            }
            else
            {
                return null;
            }
        }

        public override bool RemoveVessel(int vesselID)
        {
            if(ContainsVessel(vesselID))
            {
                Vessel vessel = vesselDictionary[vesselID];
                vesselDictionary.Remove(vesselID);
                vesselDictionaryByOwnerPlayerID.Remove(vessel.OwnerPlayerID);
                onVesselChange(vessel, DataChangeType.Remove);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void InformVesselFullDataUpdated(Vessel vessel)
        {
            onVesselChange?.Invoke(vessel, DataChangeType.Update);
        }
        private void AssemblyVessel(Vessel vessel)
        {
            vessel.OnVesselFullDataUpdated += InformVesselFullDataUpdated;
            vessel.OnVesselTransformUpdated += SystemManager.Instance.EventManager.SyncDataResolver.SyncVesselTransform;
            vessel.OnDecorationChange += SystemManager.Instance.EventManager.SyncDataResolver.SyncVesselDecorationChange;
        }
        private void DisassemblyVessel(Vessel vessel)
        {
            vessel.OnVesselFullDataUpdated -= InformVesselFullDataUpdated;
            vessel.OnVesselTransformUpdated -= SystemManager.Instance.EventManager.SyncDataResolver.SyncVesselTransform;
            vessel.OnDecorationChange -= SystemManager.Instance.EventManager.SyncDataResolver.SyncVesselDecorationChange;
        }
    }
}
