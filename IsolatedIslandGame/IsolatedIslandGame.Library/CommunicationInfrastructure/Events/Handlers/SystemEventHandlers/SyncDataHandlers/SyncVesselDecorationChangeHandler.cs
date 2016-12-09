using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncVesselDecorationChangeHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncVesselDecorationChangeHandler(SystemManager subject) : base(subject, 10)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int vesselID = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.VesselID];
                    int decorationID = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.DecorationID];
                    int materialItemID = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.MaterialItemID];
                    float positionX = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionX];
                    float positionY = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionY];
                    float positionZ = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionZ];
                    float eulerAngleX = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleX];
                    float eulerAngleY = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleY];
                    float eulerAngleZ = (int)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleZ];
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncVesselDecorationChangeParameterCode.DataChangeType];

                    Vessel vessel = VesselManager.Instance.FindVessel(vesselID);

                    Decoration decoration = new Decoration(
                            decorationID: decorationID,
                            material: ItemManager.Instance.FindItem(materialItemID) as Material,
                            position: new UnityEngine.Vector3(positionX, positionY, positionZ),
                            rotation: UnityEngine.Quaternion.Euler(eulerAngleX, eulerAngleY, eulerAngleZ));
                    switch (changeType)
                    {
                        case DataChangeType.Add:
                        case DataChangeType.Update:
                            vessel.AddDecoration(decoration);
                            break;
                        case DataChangeType.Remove:
                            vessel.RemoveDecoration(decoration.DecorationID);
                            break;
                        default:
                            LogService.Error("SyncVesselDecorationChange Error Undefined DataChangeType");
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncVesselDecorationChange Parameter Cast Error");
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
