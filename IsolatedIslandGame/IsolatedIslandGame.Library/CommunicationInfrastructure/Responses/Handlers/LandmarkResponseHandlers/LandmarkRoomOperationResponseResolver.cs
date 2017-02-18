using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers
{
    internal class LandmarkRoomOperationResponseResolver : ResponseHandler<Landmark, LandmarkOperationCode>
    {
        public LandmarkRoomOperationResponseResolver(Landmark subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            if (returnCode == ErrorCode.NoError)
            {
                if (parameters.Count != 5)
                {
                    LogService.ErrorFormat("LandmarkRoomOperationResponse Parameter Error Parameter Count: {0}", parameters.Count);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                LogService.ErrorFormat("LandmarkRoomOperationResponse Error ErrorCode: {0}, DebugMessage: {1}", returnCode, debugMessage);
                return false;
            }
        }

        internal override bool Handle(LandmarkOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                try
                {
                    int landmarkRoomID = (int)parameters[(byte)LandmarkRoomResponseParameterCode.LandmarkRoomID];
                    LandmarkRoomOperationCode resolvedOperationCode = (LandmarkRoomOperationCode)parameters[(byte)LandmarkRoomResponseParameterCode.OperationCode];
                    ErrorCode resolvedReturnCode = (ErrorCode)parameters[(byte)LandmarkRoomResponseParameterCode.ReturnCode];
                    string resolvedDebugMessage = (string)parameters[(byte)LandmarkRoomResponseParameterCode.DebugMessage];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkRoomResponseParameterCode.Parameters];

                    LandmarkRoom room;
                    if (subject.FindRoom(landmarkRoomID, out room))
                    {
                        room.ResponseManager.Operate(resolvedOperationCode, resolvedReturnCode, resolvedDebugMessage, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("LandmarkRoomOperationResponse Error LandmarkRoom ID: {0} Not in Identity: {1}", landmarkRoomID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkRoomOperationResponse Parameter Cast Error");
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
