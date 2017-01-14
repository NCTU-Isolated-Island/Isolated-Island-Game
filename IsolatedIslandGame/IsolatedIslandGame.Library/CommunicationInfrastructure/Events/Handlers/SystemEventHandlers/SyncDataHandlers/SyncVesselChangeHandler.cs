using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncVesselChangeHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncVesselChangeHandler(SystemManager subject) : base(subject, 9)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncVesselChangeParameterCode.DataChangeType];
                    int vesselID = (int)parameters[(byte)SyncVesselChangeParameterCode.VesselID];
                    int playerID = (int)parameters[(byte)SyncVesselChangeParameterCode.PlayerID];
                    string nickname = (string)parameters[(byte)SyncVesselChangeParameterCode.Nickname];
                    string signature = (string)parameters[(byte)SyncVesselChangeParameterCode.Signature];
                    GroupType groupType = (GroupType)parameters[(byte)SyncVesselChangeParameterCode.GroupType];
                    float locationX = (float)parameters[(byte)SyncVesselChangeParameterCode.LocationX];
                    float locationZ = (float)parameters[(byte)SyncVesselChangeParameterCode.LocationZ];
                    float eulerAngleY = (float)parameters[(byte)SyncVesselChangeParameterCode.EulerAngleY];

                    Vessel vessel = new Vessel(
                        vesselID: vesselID, 
                        playerInformation: new PlayerInformation
                        {
                            playerID = playerID,
                            nickname = nickname,
                            signature = signature,
                            groupType = groupType,
                            vesselID = vesselID
                        },
                        locationX: locationX,
                        locationZ: locationZ,
                        rotationEulerAngleY: eulerAngleY);
                    switch(changeType)
                    {
                        case DataChangeType.Add:
                        case DataChangeType.Update:
                            VesselManager.Instance.AddVessel(vessel);
                            break;
                        case DataChangeType.Remove:
                            VesselManager.Instance.RemoveVessel(vessel.VesselID);
                            break;
                        default:
                            LogService.FatalFormat("SyncVesselChange undefined DataChangeType: {0}", changeType);
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncVesselChange Parameter Cast Error");
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
