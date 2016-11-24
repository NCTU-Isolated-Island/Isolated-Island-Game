using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters;
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
            Dictionary<byte, object> informDataParameters = new Dictionary<byte, object>
            {
                { (byte)InformDataEventParameterCode.InformCode, (byte)informCode },
                { (byte)InformDataEventParameterCode.Parameters, parameters }
            };
            subject.EventManager.SendEvent(UserEventCode.InformData, informDataParameters);
        }
    }
}
