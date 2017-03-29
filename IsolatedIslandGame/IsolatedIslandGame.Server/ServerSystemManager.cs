using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class ServerSystemManager : SystemManager
    {
        public ServerSystemManager() : base(new ServerOperationInterface())
        {
            foreach(var worldMessage in DatabaseService.RepositoryList.WorldChannelMessageRepository.ListLatestN_Message(10))
            {
                LoadWorldChannelMessage(worldMessage);
            }
            OnLoadWorldChannelMessage += EventManager.BroadcastWorldChannelMessage;
        }

        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            LogService.FatalFormat("Server CheckSystemVersion");
        }

        public override void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            foreach(ServerUser user in UserFactory.Instance.Users)
            {
                user?.SendEvent(eventCode, parameters);
            }
        }
    }
}
