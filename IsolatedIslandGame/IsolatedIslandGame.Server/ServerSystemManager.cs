using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class ServerSystemManager : SystemManager
    {
        public override void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            foreach(ServerUser user in UserFactory.Instance.Users)
            {
                user?.SendEvent(eventCode, parameters);
            }
        }
    }
}
