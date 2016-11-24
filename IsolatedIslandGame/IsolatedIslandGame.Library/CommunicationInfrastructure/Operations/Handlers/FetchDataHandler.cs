using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public abstract class FetchDataHandler<TSubject, TFetchDataCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;
        protected int correctParameterCount;

        protected FetchDataHandler(TSubject subject, int correctParameterCount)
        {
            this.subject = subject;
            this.correctParameterCount = correctParameterCount;
        }

        public virtual bool Handle(TFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            string debugMessage;
            if (CheckParameter(parameter, out debugMessage))
            {
                return true;
            }
            else
            {
                SendError(fetchCode, ErrorCode.ParameterError, debugMessage);
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
        public abstract void SendResponse(TFetchDataCode fetchCode, Dictionary<byte, object> parameters);
        public abstract void SendError(TFetchDataCode fetchCode, ErrorCode errorCode, string debugMessage);
    }
}
