using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    public class SystemSyncDataResolver : SyncDataResolver<SystemManager, SystemEventCode, SystemSyncDataCode>
    {
        internal SystemSyncDataResolver(SystemManager systemManager) : base(systemManager)
        {
            syncTable.Add(SystemSyncDataCode.VesselChange, new SyncVesselChangeHandler(subject));
            syncTable.Add(SystemSyncDataCode.VesselTransform, new SyncVesselTransformHandler(subject));
            syncTable.Add(SystemSyncDataCode.VesselDecorationChange, new SyncVesselDecorationChangeHandler(subject));
        }

        internal override void SendSyncData(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncVesselChange(Vessel vessel, DataChangeType changeType)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselChangeParameterCode.VesselID, vessel.VesselID },
                { (byte)SyncVesselChangeParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                { (byte)SyncVesselChangeParameterCode.Name, vessel.Name },
                { (byte)SyncVesselChangeParameterCode.LocationX, vessel.LocationX },
                { (byte)SyncVesselChangeParameterCode.LocationZ, vessel.LocationZ },
                { (byte)SyncVesselChangeParameterCode.EulerAngleY, vessel.RotationEulerAngleY },
                { (byte)SyncVesselChangeParameterCode.DataChangeType, (byte)changeType }
            };
            SendSyncData(SystemSyncDataCode.VesselChange, parameters);
        }
        public void SyncVesselTransform(int vesselID, float locationX, float locationY, float rotationEulerAngleY)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselTransformParameterCode.VesselID, vesselID },
                { (byte)SyncVesselTransformParameterCode.LocationX, locationX },
                { (byte)SyncVesselTransformParameterCode.LocationZ, locationY },
                { (byte)SyncVesselTransformParameterCode.EulerAngleY, rotationEulerAngleY }
            };
            SendSyncData(SystemSyncDataCode.VesselTransform, parameters);
        }
        public void SyncVesselDecorationChange(int vesselID, Decoration decoration, DataChangeType changeType)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselDecorationChangeParameterCode.VesselID, vesselID },
                { (byte)SyncVesselDecorationChangeParameterCode.DecorationID, decoration.DecorationID },
                { (byte)SyncVesselDecorationChangeParameterCode.MaterialItemID, decoration.Material.ItemID },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionX, decoration.PositionX },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionY, decoration.PositionY },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionZ, decoration.PositionZ },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleX, decoration.RotationEulerAngleX },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleY, decoration.RotationEulerAngleY },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleZ, decoration.RotationEulerAngleZ },
                { (byte)SyncVesselDecorationChangeParameterCode.DataChangeType, (byte)changeType }
            };
            SendSyncData(SystemSyncDataCode.VesselDecorationChange, parameters);
        }
    }
}
