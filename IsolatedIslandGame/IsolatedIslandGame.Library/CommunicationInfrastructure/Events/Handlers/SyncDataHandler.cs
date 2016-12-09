using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers
{
    internal abstract class SyncDataHandler<TSubject, TSyncDataCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;
        protected int correctParameterCount;

        protected SyncDataHandler(TSubject subject, int correctParameterCount)
        {
            this.subject = subject;
            this.correctParameterCount = correctParameterCount;
        }

        internal virtual bool Handle(TSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            string debugMessage;
            if (CheckParameter(parameters, out debugMessage))
            {
                return true;
            }
            else
            {
                LogService.ErrorFormat("{0} SyncData Parameter Error On {1} Identity: {2}, Debug Message: {3}", subject.GetType(), syncCode, subject.IdentityInformation, debugMessage);
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
    }
}
