using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers
{
    public class UserSyncDataResolver : SyncDataResolver<User, UserEventCode, UserSyncDataCode>
    {
        internal UserSyncDataResolver(User user) : base(user)
        {
        }
        internal override void SendSyncData(UserSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }
    }
}
