using IsolatedIslandGame.Protocol.Communication.EventParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers
{
    public abstract class InformDataResolver<TSubject, TEventCode, TInformDataCode> : EventHandler<TSubject, TEventCode> where TSubject : IIdentityProvidable
    {
        internal readonly Dictionary<TInformDataCode, InformDataHandler<TSubject, TInformDataCode>> informTable;

        internal InformDataResolver(TSubject subject) : base(subject, 2)
        {
            informTable = new Dictionary<TInformDataCode, InformDataHandler<TSubject, TInformDataCode>>();
        }

        internal override bool Handle(TEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                TInformDataCode informCode = (TInformDataCode)parameters[(byte)InformDataEventParameterCode.InformCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)InformDataEventParameterCode.Parameters];
                if (informTable.ContainsKey(informCode))
                {
                    return informTable[informCode].Handle(informCode, resolvedParameters);
                }
                else
                {
                    LogService.ErrorFormat("{0} InformData Event Not Exist Inform Code: {}", subject.GetType(), informCode);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        internal abstract void SendInform(TInformDataCode informCode, Dictionary<byte, object> parameters);
    }
}
