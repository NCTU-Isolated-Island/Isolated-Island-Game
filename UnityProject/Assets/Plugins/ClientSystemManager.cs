using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;
using UnityEngine;

namespace IsolatedIslandGame.Client
{
    public class ClientSystemManager : SystemManager
    {
        public static void Initial(User user)
        {
            Initial(new ClientSystemManager(user));
        }

        private ClientSystemManager(User user) : base()
        {
            LogService.InitialService(Debug.Log, Debug.LogFormat, Debug.LogWarning, Debug.LogWarningFormat, Debug.LogError, Debug.LogErrorFormat);
        }

        public override void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("Client SendAllUserEvent UserEventCode: {0}", eventCode);
        }
    }
}
