using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers
{
    internal class LandmarkOperationResponseResolver : ResponseHandler<SystemManager, SystemOperationCode>
    {
        public LandmarkOperationResponseResolver(SystemManager subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            if (returnCode == ErrorCode.NoError)
            {
                if (parameters.Count != 5)
                {
                    LogService.ErrorFormat("LandmarkOperationResponse Parameter Error Parameter Count: {0}", parameters.Count);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                LogService.ErrorFormat("LandmarkOperationResponse Error ErrorCode: {0}, DebugMessage: {1}", returnCode, debugMessage);
                return false;
            }
        }

        internal override bool Handle(SystemOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                try
                {
                    int landmarkID = (int)parameters[(byte)LandmarkResponseParameterCode.LandmarkID];
                    LandmarkOperationCode resolvedOperationCode = (LandmarkOperationCode)parameters[(byte)LandmarkResponseParameterCode.OperationCode];
                    ErrorCode resolvedReturnCode = (ErrorCode)parameters[(byte)LandmarkResponseParameterCode.ReturnCode];
                    string resolvedDebugMessage = (string)parameters[(byte)LandmarkResponseParameterCode.DebugMessage];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkResponseParameterCode.Parameters];

                    Landmark landmark;
                    if(LandmarkManager.Instance.FindLandmark(landmarkID, out landmark))
                    {
                        landmark.ResponseManager.Operate(resolvedOperationCode, resolvedReturnCode, resolvedDebugMessage, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("LandmarkOperationResponse Error Landmark ID: {0} Not in Identity: {1}", landmarkID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkOperationResponse Parameter Cast Error");
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
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
