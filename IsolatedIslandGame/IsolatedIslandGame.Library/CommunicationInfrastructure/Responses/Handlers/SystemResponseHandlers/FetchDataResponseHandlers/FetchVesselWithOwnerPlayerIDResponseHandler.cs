using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchVesselWithOwnerPlayerIDResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchVesselWithOwnerPlayerIDResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 6)
                        {
                            LogService.ErrorFormat(string.Format("FetchVesselWithOwnerPlayerIDResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchVesselWithOwnerPlayerIDResponse Error DebugMessage: {0}", debugMessage);
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
                    int vesselID = (int)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.VesselID];
                    int ownerPlayerID = (int)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.OwnerPlayerID];
                    string ownerName = (string)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.Name];
                    float locationX = (float)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.LocationX];
                    float locationZ = (float)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.LocationZ];
                    float eulerAngleY = (float)parameters[(byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.EulerAngleY];
                    VesselManager.Instance.AddVessel(new Vessel(vesselID, ownerPlayerID, ownerName, locationX, locationZ, eulerAngleY));
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchVesselWithOwnerPlayerIDResponse Parameter Cast Error");
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
