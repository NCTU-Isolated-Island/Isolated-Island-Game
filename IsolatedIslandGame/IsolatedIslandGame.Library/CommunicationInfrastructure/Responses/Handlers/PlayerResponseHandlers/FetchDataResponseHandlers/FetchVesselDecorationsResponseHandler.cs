using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchVesselDecorationsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchVesselDecorationsResponseHandler(Player subject) : base(subject)
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

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
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
                    if (subject.Vessel.VesselID == vesselID)
                    {
                        subject.Vessel.AddDecoration(new Decoration(
                            decorationID: decorationID,
                            material: ItemManager.Instance.FindItem(materialItemID) as Material,
                            positionX: positionX,
                            positionY: positionY,
                            positionZ: positionZ,
                            rotationEulerAngleX: eulerAngleX,
                            rotationEulerAngleY: eulerAngleY,
                            rotationEulerAngleZ: eulerAngleZ));
                        return true;
                    }
                    else
                    {
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
