﻿using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncVesselTransformHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncVesselTransformHandler(SystemManager subject) : base(subject, 5)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int vesselID = (int)parameters[(byte)SyncVesselTransformParameterCode.VesselID];
                    float locationX = (float)parameters[(byte)SyncVesselTransformParameterCode.LocationX];
                    float locationZ = (float)parameters[(byte)SyncVesselTransformParameterCode.LocationZ];
                    float eulerAngleY = (float)parameters[(byte)SyncVesselTransformParameterCode.EulerAngleY];
                    OceanType locatedOceanType = (OceanType)parameters[(byte)SyncVesselTransformParameterCode.LocatedOceanType];

                    Vessel vessel;
                    if(VesselManager.Instance.FindVessel(vesselID, out vessel))
                    {
                        vessel.UpdateTransform(locationX, locationZ, eulerAngleY, locatedOceanType);
                        return true;
                    }
                    else
                    {
                        LogService.Error($"SyncVesselTransform Error, Can't Find Vessel ID: {vesselID}");
                        return false;
                    }
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
