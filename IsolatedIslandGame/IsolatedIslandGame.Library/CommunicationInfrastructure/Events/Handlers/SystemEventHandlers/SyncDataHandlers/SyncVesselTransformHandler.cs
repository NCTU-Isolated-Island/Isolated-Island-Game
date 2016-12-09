using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncVesselTransformHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncVesselTransformHandler(SystemManager subject) : base(subject, 6)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int vesselID = (int)parameters[(byte)SyncVesselTransformParameterCode.VesselID];
                    float locationX = (int)parameters[(byte)SyncVesselTransformParameterCode.LocationX];
                    float locationZ = (int)parameters[(byte)SyncVesselTransformParameterCode.LocationZ];
                    float eulerAngleX = (int)parameters[(byte)SyncVesselTransformParameterCode.EulerAngleX];
                    float eulerAngleY = (int)parameters[(byte)SyncVesselTransformParameterCode.EulerAngleY];
                    float eulerAngleZ = (int)parameters[(byte)SyncVesselTransformParameterCode.EulerAngleZ];

                    Vessel vessel = VesselManager.Instance.FindVessel(vesselID);
                    vessel.UpdateTransform(locationX, locationZ, UnityEngine.Quaternion.Euler(eulerAngleX, eulerAngleY, eulerAngleZ));

                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncVesselTransform Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
