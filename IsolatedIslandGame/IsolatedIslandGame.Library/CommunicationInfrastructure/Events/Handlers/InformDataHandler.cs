using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers
{
    internal abstract class InformDataHandler<TSubject, TInformDataCode> where TSubject : IIdentityProvidable
    {
        protected TSubject subject;
        protected int correctParameterCount;

        protected InformDataHandler(TSubject subject, int correctParameterCount)
        {
            this.subject = subject;
            this.correctParameterCount = correctParameterCount;
        }

        public virtual bool Handle(TInformDataCode informCode, Dictionary<byte, object> parameter)
        {
            string debugMessage;
            if (CheckParameter(parameter, out debugMessage))
            {
                return true;
            }
            else
            {
                LogService.ErrorFormat("{0} InformData Parameter Error On {1} Identity: {2}, Debug Message: {3}", subject.GetType(), informCode, subject.IdentityInformation, debugMessage);
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
