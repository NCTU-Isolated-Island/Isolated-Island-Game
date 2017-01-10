using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers
{
    public abstract class ResponseHandler<TSubject, TOperationCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;

        protected ResponseHandler(TSubject subject)
        {
            this.subject = subject;
        }

        internal virtual bool Handle(TOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (CheckError(parameters, returnCode, debugMessage))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal abstract bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage);
    }
}
