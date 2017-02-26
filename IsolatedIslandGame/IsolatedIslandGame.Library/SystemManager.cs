using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public abstract class SystemManager : IIdentityProvidable
    {
        public static SystemManager Instance { get; private set; }
        public static void Initial(SystemManager manager)
        {
            Instance = manager;
        }

        public SystemEventManager EventManager { get; private set; }
        public SystemOperationManager OperationManager { get; private set; }
        public SystemResponseManager ResponseManager { get; private set; }

        public string IdentityInformation
        {
            get
            {
                return "Local System";
            }
        }

        protected SystemManager()
        {
            EventManager = new SystemEventManager(this);
            OperationManager = new SystemOperationManager(this);
            ResponseManager = new SystemResponseManager(this);
        }
        public abstract void SendAllUserEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
    }
}
