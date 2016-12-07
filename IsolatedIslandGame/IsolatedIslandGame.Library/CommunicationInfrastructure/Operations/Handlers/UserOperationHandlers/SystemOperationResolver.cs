using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    internal class SystemOperationResolver : UserOperationHandler
    {
        internal SystemOperationResolver(User subject) : base(subject, 2)
        {
        }

        internal override bool Handle(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                try
                {
                    SystemOperationCode resolvedOperationCode = (SystemOperationCode)parameters[(byte)SystemOperationParameterCode.OperationCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)SystemOperationParameterCode.Parameters];
                    SystemManager.Instance.OperationManager.Operate(resolvedOperationCode, resolvedParameters);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("SystemOperation Parameter Cast Error");
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
