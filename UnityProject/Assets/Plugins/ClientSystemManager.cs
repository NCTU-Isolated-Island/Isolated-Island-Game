using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IsolatedIslandGame.Client
{
    public class ClientSystemManager : SystemManager
    {
        public static void Initial(User user)
        {
            Initial(new ClientSystemManager(user));
        }

        private ClientSystemManager(User user) : base(null)
        {
            LogService.InitialService(Debug.Log, Debug.LogFormat, Debug.LogWarning, Debug.LogWarningFormat, Debug.LogError, Debug.LogErrorFormat);
        }

        public override void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("Client SendAllUserEvent UserEventCode: {0}", eventCode);
        }

        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            if (SystemConfiguration.Instance.ServerVersion != serverVersion || SystemConfiguration.Instance.ClientVersion != clientVersion)
            {
                UserManager.Instance.User.UserInform("提示", "有新版本，請更新以獲取最新的遊戲內容");
                LogService.FatalFormat("ServerVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ServerVersion, serverVersion);
                LogService.FatalFormat("ClientVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ClientVersion, clientVersion);
            }
            else
            {
                LogService.InfoFormat("ServerVersion: {0}", serverVersion);
            }
        }
    }
}
