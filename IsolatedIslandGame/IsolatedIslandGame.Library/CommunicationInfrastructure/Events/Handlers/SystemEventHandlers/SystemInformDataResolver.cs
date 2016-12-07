using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters;
using IsolatedIslandGame.Protocol.Communication.InformDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    public class SystemInformDataResolver : InformDataResolver<SystemManager, SystemEventCode, SystemInformDataCode>
    {
        internal SystemInformDataResolver(SystemManager systemManager) : base(systemManager)
        {
        }

        internal override void SendInform(SystemInformDataCode informCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> informDataParameters = new Dictionary<byte, object>
            {
                { (byte)InformDataEventParameterCode.InformCode, (byte)informCode },
                { (byte)InformDataEventParameterCode.Parameters, parameters }
            };
            subject.EventManager.SendEvent(SystemEventCode.InformData, informDataParameters);
        }
    }
}
