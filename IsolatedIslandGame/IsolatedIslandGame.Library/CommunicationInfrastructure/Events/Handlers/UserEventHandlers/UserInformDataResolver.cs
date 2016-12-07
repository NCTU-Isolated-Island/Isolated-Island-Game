using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.InformDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers
{
    public class UserInformDataResolver : InformDataResolver<User, UserEventCode, UserInformDataCode>
    {
        internal UserInformDataResolver(User user) : base(user)
        {
        }
        internal override void SendInform(UserInformDataCode informCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendInformDataEvent(informCode, parameters);
        }
    }
}
