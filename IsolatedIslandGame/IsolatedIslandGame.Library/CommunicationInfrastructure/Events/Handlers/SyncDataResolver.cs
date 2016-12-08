using IsolatedIslandGame.Protocol.Communication.EventParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers
{
    public abstract class SyncDataResolver<TSubject, TEventCode, TSyncDataCode> : EventHandler<TSubject, TEventCode> where TSubject : IIdentityProvidable
    {
        internal readonly Dictionary<TSyncDataCode, SyncDataHandler<TSubject, TSyncDataCode>> syncTable;

        internal SyncDataResolver(TSubject subject) : base(subject, 2)
        {
            syncTable = new Dictionary<TSyncDataCode, SyncDataHandler<TSubject, TSyncDataCode>>();
        }

        internal override bool Handle(TEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                TSyncDataCode syncCode = (TSyncDataCode)parameters[(byte)SyncDataEventParameterCode.SyncCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)SyncDataEventParameterCode.Parameters];
                if (syncTable.ContainsKey(syncCode))
                {
                    return syncTable[syncCode].Handle(syncCode, resolvedParameters);
                }
                else
                {
                    LogService.ErrorFormat("{0} SyncData Event Not Exist SyncCode: {1}", subject.GetType(), syncCode);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        internal abstract void SendSyncData(TSyncDataCode syncCode, Dictionary<byte, object> parameters);
    }
}
