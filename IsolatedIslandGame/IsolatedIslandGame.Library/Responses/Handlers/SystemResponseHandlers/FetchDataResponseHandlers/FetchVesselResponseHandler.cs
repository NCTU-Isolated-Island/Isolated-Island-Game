using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchVesselResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchVesselResponseHandler(SystemManager subject) : base(subject)
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

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int vesselID = (int)parameters[(byte)FetchVesselResponseParameterCode.VesselID];
                    int playerID = (int)parameters[(byte)FetchVesselResponseParameterCode.PlayerID];
                    string nickname = (string)parameters[(byte)FetchVesselResponseParameterCode.Nickname];
                    string signature = (string)parameters[(byte)FetchVesselResponseParameterCode.Signature];
                    GroupType groupType = (GroupType)parameters[(byte)FetchVesselResponseParameterCode.GroupType];
                    float locationX = (float)parameters[(byte)FetchVesselResponseParameterCode.LocationX];
                    float locationZ = (float)parameters[(byte)FetchVesselResponseParameterCode.LocationZ];
                    float eulerAngleY = (float)parameters[(byte)FetchVesselResponseParameterCode.EulerAngleY];

                    VesselManager.Instance.AddVessel(new Vessel(
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
                        rotationEulerAngleY: eulerAngleY));
                    return true;
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
