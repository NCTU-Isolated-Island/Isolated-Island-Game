using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers
{
    public abstract class FetchDataResponseHandler<TSubject, TFetchDataCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;

        protected FetchDataResponseHandler(TSubject subject)
        {
            this.subject = subject;
        }

        public virtual bool Handle(TFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (CheckError(parameters, returnCode, fetchDebugMessage))
            {
                return true;
            }
            else
            {
                LogService.ErrorFormat("{0} FetchData Parameter Error On {1}, Identity: {2}, Debug Message: {3}", subject.GetType(), fetchCode, subject.IdentityInformation, fetchDebugMessage);
                return false;
            }
        }
        public abstract bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage);
    }
}
