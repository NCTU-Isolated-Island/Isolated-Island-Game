using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchVesselResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchVesselResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 8)
                        {
                            LogService.ErrorFormat(string.Format("FetchVesselResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchVesselResponse Error DebugMessage: {0}", debugMessage);
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
                    int vesselID = (int)parameters[(byte)FetchVesselResponseParameterCode.VesselID];
                    int ownerPlayerID = (int)parameters[(byte)FetchVesselResponseParameterCode.OwnerPlayerID];
                    string ownerName = (string)parameters[(byte)FetchVesselResponseParameterCode.Name];
                    float locationX = (float)parameters[(byte)FetchVesselResponseParameterCode.LocationX];
                    float locationZ = (float)parameters[(byte)FetchVesselResponseParameterCode.LocationZ];
                    float eulerAngleX = (float)parameters[(byte)FetchVesselResponseParameterCode.EulerAngleX];
                    float eulerAngleY = (float)parameters[(byte)FetchVesselResponseParameterCode.EulerAngleY];
                    float eulerAngleZ = (float)parameters[(byte)FetchVesselResponseParameterCode.EulerAngleZ];
                    if(subject.PlayerID == ownerPlayerID)
                    {
                        subject.BindVessel(new Vessel(vesselID, ownerPlayerID, ownerName, locationX, locationZ, Quaternion.Euler(eulerAngleX, eulerAngleY, eulerAngleZ)));
                        subject.OperationManager.FetchDataResolver.FetchVesselDecorations(subject.Vessel.VesselID);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchVesselResponse Parameter Cast Error");
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
