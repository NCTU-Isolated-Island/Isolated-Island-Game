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
                vesselDictionaryByOwnerPlayerID.Add(vessel.PlayerInformation.playerID, vessel);
                AssemblyVessel(vessel);
                onVesselChange(DataChangeType.Add, vessel);
            }
        }

        public override bool FindVessel(int vesselID, out Vessel vessel)
        {
            if (ContainsVessel(vesselID))
            {
                vessel = vesselDictionary[vesselID];
                return true;
            }
            else
            {
                vessel = null;
                return false;
            }
        }

        public override bool FindVesselByOwnerPlayerID(int ownerPlayerID, out Vessel vessel)
        {
            if(ContainsVesselWithOwnerPlayerID(ownerPlayerID))
            {
                vessel = vesselDictionaryByOwnerPlayerID[ownerPlayerID];
                return true;
            }
            else
            {
                vessel = null;
                return false;
            }
        }

        public override bool RemoveVessel(int vesselID)
        {
            if(ContainsVessel(vesselID))
            {
                Vessel vessel = vesselDictionary[vesselID];
                vesselDictionary.Remove(vesselID);
                vesselDictionaryByOwnerPlayerID.Remove(vessel.PlayerInformation.playerID);
                DisassemblyVessel(vessel);
                onVesselChange(DataChangeType.Remove, vessel);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void InformVesselFullDataUpdated(Vessel vessel)
        {
            onVesselChange?.Invoke(DataChangeType.Update, vessel);
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
