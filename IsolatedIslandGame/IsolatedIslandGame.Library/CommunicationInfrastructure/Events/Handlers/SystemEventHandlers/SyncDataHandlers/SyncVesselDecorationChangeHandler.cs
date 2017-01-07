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
                    float positionX = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionX];
                    float positionY = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionY];
                    float positionZ = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.PositionZ];
                    float eulerAngleX = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleX];
                    float eulerAngleY = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleY];
                    float eulerAngleZ = (float)parameters[(byte)SyncVesselDecorationChangeParameterCode.EulerAngleZ];
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncVesselDecorationChangeParameterCode.DataChangeType];

                    Vessel vessel;
                    if(VesselManager.Instance.FindVessel(vesselID, out vessel))
                    {
                        Item material;
                        if(ItemManager.Instance.FindItem(materialItemID, out material))
                        {
                            Decoration decoration = new Decoration(
                            decorationID: decorationID,
                            material: material as Material,
                            positionX: positionX,
                            positionY: positionY,
                            positionZ: positionZ,
                            rotationEulerAngleX: eulerAngleX,
                            rotationEulerAngleY: eulerAngleY,
                            rotationEulerAngleZ: eulerAngleZ);
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
                        else
                        {
                            LogService.Error($"SyncVesselDecorationChange Error, Item not existed ItemID: {materialItemID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"SyncVesselDecorationChange Error, Vessel not existed, VesselID: {vesselID}");
                        return false;
                    }
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
