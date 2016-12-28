using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchVesselDecorationsResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchVesselDecorationsResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 9)
                        {
                            LogService.ErrorFormat(string.Format("FetchVesselDecorationsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchVesselDecorationsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int vesselID = (int)parameters[(byte)FetchVesselDecorationsResponseParameterCode.VesselID];
                    int decorationID = (int)parameters[(byte)FetchVesselDecorationsResponseParameterCode.DecorationID];
                    int materialItemID = (int)parameters[(byte)FetchVesselDecorationsResponseParameterCode.MaterialItemID];
                    float positionX = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.PositionX];
                    float positionY = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.PositionY];
                    float positionZ = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.PositionZ];
                    float eulerAngleX = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.EulerAngleX];
                    float eulerAngleY = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.EulerAngleY];
                    float eulerAngleZ = (float)parameters[(byte)FetchVesselDecorationsResponseParameterCode.EulerAngleZ];
                    Vessel vessel;
                    if (VesselManager.Instance.FindVessel(vesselID, out vessel))
                    {
                        Item material;
                        if (ItemManager.Instance.FindItem(materialItemID, out material))
                        {
                            if(material is Material)
                            {
                                vessel.AddDecoration(new Decoration(
                                decorationID: decorationID,
                                material: material as Material,
                                positionX: positionX,
                                positionY: positionY,
                                positionZ: positionZ,
                                rotationEulerAngleX: eulerAngleX,
                                rotationEulerAngleY: eulerAngleY,
                                rotationEulerAngleZ: eulerAngleZ));
                            }
                            else
                            {
                                Material specializedMaterial;
                                if(ItemManager.Instance.SpecializeItemToMaterial(materialItemID, out specializedMaterial))
                                {
                                    vessel.AddDecoration(new Decoration(
                                    decorationID: decorationID,
                                    material: specializedMaterial,
                                    positionX: positionX,
                                    positionY: positionY,
                                    positionZ: positionZ,
                                    rotationEulerAngleX: eulerAngleX,
                                    rotationEulerAngleY: eulerAngleY,
                                    rotationEulerAngleZ: eulerAngleZ));
                                }
                                else
                                {
                                    LogService.Error($"FetchVesselDecorationsResponse Error, SpecializeItemToMaterial Fail, VesselID: {vesselID}, MaterialItemID: {materialItemID}");
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            LogService.Error($"FetchVesselDecorationsResponse Error, Material not existed, VesselID: {vesselID}, MaterialItemID: {materialItemID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"FetchVesselDecorationsResponse Error, Vessel not existed, VesselID: {vesselID}, DecorationID: {decorationID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchVesselDecorationsResponse Parameter Cast Error");
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
