using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public abstract class SystemManager : IIdentityProvidable
    {
        public static SystemManager Instance { get; private set; }
        public static void Initial(SystemManager manager)
        {
            Instance = manager;
        }

        internal OperationInterface OperationInterface { get; private set; }
        public SystemEventManager EventManager { get; private set; }
        public SystemOperationManager OperationManager { get; private set; }
        public SystemResponseManager ResponseManager { get; private set; }

        private Dictionary<int, WorldChannelMessage> worldChannelMessages = new Dictionary<int, WorldChannelMessage>();

        private event Action<WorldChannelMessage> onLoadWorldChannelMessage;
        public event Action<WorldChannelMessage> OnLoadWorldChannelMessage { add { onLoadWorldChannelMessage += value; } remove { onLoadWorldChannelMessage -= value; }  }

        public string IdentityInformation
        {
            get
            {
                return "Local System";
            }
        }

        protected SystemManager(OperationInterface operationInterface)
        {
            EventManager = new SystemEventManager(this);
            OperationManager = new SystemOperationManager(this);
            ResponseManager = new SystemResponseManager(this);
            OperationInterface = operationInterface;
        }
        public abstract void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
        public List<WorldChannelMessage> GetWorldChannelMessages(int maxCount = 0)
        {
            if(maxCount > 0)
            {
                lock (worldChannelMessages)
                {
                    return worldChannelMessages.Values.OrderBy(x => x.Message.sendTime).Reverse().Take(maxCount).ToList();
                }
            }
            else
            {
                lock (worldChannelMessages)
                {
                    return worldChannelMessages.Values.OrderBy(x => x.Message.sendTime).Reverse().ToList();
                }
            }
        }
        public void LoadWorldChannelMessage(WorldChannelMessage worldMessage)
        {
            if(!worldChannelMessages.ContainsKey(worldMessage.WorldChannelMessageID))
            {
                lock(worldChannelMessages)
                {
                    worldChannelMessages.Add(worldMessage.WorldChannelMessageID, worldMessage);
                }
                onLoadWorldChannelMessage?.Invoke(worldMessage);
            }
        }
        public abstract void CheckSystemVersion(string serverVersion, string clientVersion);
    }
}
