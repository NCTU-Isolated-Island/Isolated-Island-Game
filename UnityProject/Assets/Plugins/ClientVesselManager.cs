using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using UnityEngine;

public class ClientVesselManager : VesselManager
{
    private event VesselChangeEventHandler onVesselChange;
    public override event VesselChangeEventHandler OnVesselChange { add { onVesselChange += value; } remove { onVesselChange -= value; } }

    private event Vessel.VesselTransformUpdatedEventHandler onVesselTransformUpdated;
    public override event Vessel.VesselTransformUpdatedEventHandler OnVesselTransformUpdated { add { onVesselTransformUpdated += value; } remove { onVesselTransformUpdated -= value; } }

    private event Vessel.DecorationChangeEventHandler onVesselDecorationChange;
    public override event Vessel.DecorationChangeEventHandler OnVesselDecorationChange { add { onVesselDecorationChange += value; } remove { onVesselDecorationChange -= value; } }

    public override void AddVessel(Vessel vessel)
    {
        if(ContainsVessel(vessel.VesselID) && ContainsVesselWithOwnerPlayerID(vessel.OwnerPlayerID))
        {
            Vessel existedVessel = vesselDictionary[vessel.VesselID];
            existedVessel.UpdateTransform(vessel.LocationX, vessel.LocationZ, vessel.RotationEulerAngleY);
        }
        else if(ContainsVessel(vessel.VesselID))
        {
            Vessel existedVessel = vesselDictionary[vessel.VesselID];
            vesselDictionaryByOwnerPlayerID.Add(vessel.OwnerPlayerID, vessel);
            existedVessel.UpdateFullData(vessel);
        }
        else if (ContainsVesselWithOwnerPlayerID(vessel.OwnerPlayerID))
        {
            Vessel existedVessel = vesselDictionaryByOwnerPlayerID[vessel.OwnerPlayerID];
            vesselDictionary.Add(vessel.VesselID, vessel);
            existedVessel.UpdateFullData(vessel);
        }
        else
        {
            vesselDictionary.Add(vessel.VesselID, vessel);
            vesselDictionaryByOwnerPlayerID.Add(vessel.OwnerPlayerID, vessel);
            AssemblyVessel(vessel);
            if (onVesselChange != null)
            {
                onVesselChange(vessel, DataChangeType.Add);
            }
        }
        SystemManager.Instance.OperationManager.FetchDataResolver.FetchVesselDecorations(vessel.VesselID);
    }

    public override bool FindVessel(int vesselID, out Vessel vessel)
    {
        if(ContainsVessel(vesselID))
        {
            vessel = vesselDictionary[vesselID];
            return true;
        }
        else
        {
            vessel = new Vessel(vesselID, 0, "", 0, 0, 0);
            vesselDictionary.Add(vessel.VesselID, vessel);
            AssemblyVessel(vessel);
            if (onVesselChange != null)
            {
                onVesselChange(vessel, DataChangeType.Add);
            }
            SystemManager.Instance.OperationManager.FetchDataResolver.FetchVessel(vesselID);
            return true;
        }
    }

    public override bool FindVesselByOwnerPlayerID(int ownerPlayerID, out Vessel vessel)
    {
        if (ContainsVesselWithOwnerPlayerID(ownerPlayerID))
        {
            vessel = vesselDictionaryByOwnerPlayerID[ownerPlayerID];
            return true;
        }
        else
        {
            vessel = new Vessel(0, ownerPlayerID, "", 0, 0, 0);
            vesselDictionaryByOwnerPlayerID.Add(vessel.OwnerPlayerID, vessel);
            AssemblyVessel(vessel);
            if (onVesselChange != null)
            {
                onVesselChange(vessel, DataChangeType.Add);
            }
            SystemManager.Instance.OperationManager.FetchDataResolver.FetchVesselWithOwnerPlayerID(ownerPlayerID);
            return true;
        }
    }

    public override bool RemoveVessel(int vesselID)
    {
        if (ContainsVessel(vesselID))
        {
            Vessel vessel = vesselDictionary[vesselID];
            vesselDictionary.Remove(vesselID);
            vesselDictionaryByOwnerPlayerID.Remove(vessel.OwnerPlayerID);
            if (onVesselChange != null)
            {
                onVesselChange(vessel, DataChangeType.Remove);
            }
            DisassemblyVessel(vessel);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AssemblyVessel(Vessel vessel)
    {
        vessel.OnVesselFullDataUpdated += InformVesselFullDataUpdated;
        vessel.OnVesselTransformUpdated += InformVesselTransformUpdated;
        vessel.OnDecorationChange += InformVesselDecorationChange;
    }
    private void DisassemblyVessel(Vessel vessel)
    {
        vessel.OnVesselFullDataUpdated -= InformVesselFullDataUpdated;
        vessel.OnVesselTransformUpdated -= InformVesselTransformUpdated;
        vessel.OnDecorationChange -= InformVesselDecorationChange;
    }
    private void InformVesselFullDataUpdated(Vessel vessel)
    {
        if(onVesselChange != null)
        {
            onVesselChange(vessel, DataChangeType.Update);
        }
    }
    private void InformVesselTransformUpdated(int vesselID, float locationX, float locationY, float rotationEulerAngleY)
    {
        if (onVesselTransformUpdated != null)
        {
            onVesselTransformUpdated(vesselID, locationX, locationY, rotationEulerAngleY);
        }
    }
    private void InformVesselDecorationChange(int vesselID, Decoration decoration, DataChangeType changeType)
    {
        if(onVesselDecorationChange != null)
        {
            onVesselDecorationChange(vesselID, decoration, changeType);
        }
    }
}
