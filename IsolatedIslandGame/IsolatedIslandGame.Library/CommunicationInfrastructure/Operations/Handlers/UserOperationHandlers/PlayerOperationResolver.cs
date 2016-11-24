using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    internal class PlayerOperationResolver : UserOperationHandler
    {
        internal PlayerOperationResolver(User subject) : base(subject, 3)
        {
        }

        internal override bool Handle(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)PlayerOperationParameterCode.PlayerID];
                    PlayerOperationCode resolvedOperationCode = (PlayerOperationCode)parameters[(byte)PlayerOperationParameterCode.OperationCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)PlayerOperationParameterCode.Parameters];
                    if (subject.Player.PlayerID == playerID)
                    {
                        subject.Player.OperationManager.Operate(resolvedOperationCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("PlayerOperation Error Player ID: {0} Not in Identity: {1}", playerID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("PlayerOperation Parameter Cast Error");
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
