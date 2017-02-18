using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers
{
    internal class LandmarkRoomOperationResolver : LandmarkOperationHandler
    {
        internal LandmarkRoomOperationResolver(Landmark subject) : base(subject, 3)
        {
        }

        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int landmarkRoomID = (int)parameters[(byte)LandmarkRoomOperationParameterCode.LandmarkRoomID];
                    LandmarkRoomOperationCode resolvedOperationCode = (LandmarkRoomOperationCode)parameters[(byte)LandmarkRoomOperationParameterCode.OperationCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkRoomOperationParameterCode.Parameters];

                    LandmarkRoom room;
                    if (subject.FindRoom(landmarkRoomID, out room))
                    {
                        room.OperationManager.Operate(communicationInterface, resolvedOperationCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("LandmarkRoomOperation Error LandmarkRoom ID: {0} Not in Identity: {1}", landmarkRoomID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkRoomOperation Parameter Cast Error");
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
