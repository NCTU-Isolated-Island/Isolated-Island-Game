using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public abstract class OperationHandler<TSubject, TOperationCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;
        protected int correctParameterCount;

        internal OperationHandler(TSubject subject, int correctParameterCount)
        {
            this.subject = subject;
            this.correctParameterCount = correctParameterCount;
        }

        internal virtual bool Handle(TOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            string debugMessage;
            if (CheckParameter(parameters, out debugMessage))
            {
                return true;
            }
            else
            {
                SendError(operationCode, ErrorCode.ParameterError, debugMessage);
                return false;
            }
        }
        internal virtual bool CheckParameter(Dictionary<byte, object> parameters, out string debugMessage)
        {
            if (parameters.Count != correctParameterCount)
            {
                debugMessage = string.Format("Parameter Count: {0} Should be {1}", parameters.Count, correctParameterCount);
                return false;
            }
            else
            {
                debugMessage = "";
                return true;
            }
        }
        internal virtual void SendError(TOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            LogService.ErrorFormat("Error On {0} Identity: {1} Operation: {2}, ErrorCode: {3}, Debug Message: {4}", subject.GetType(), subject.IdentityInformation, operationCode, errorCode, debugMessage);
        }
        internal abstract void SendResponse(TOperationCode operationCode, Dictionary<byte, object> parameter);
    }
}
