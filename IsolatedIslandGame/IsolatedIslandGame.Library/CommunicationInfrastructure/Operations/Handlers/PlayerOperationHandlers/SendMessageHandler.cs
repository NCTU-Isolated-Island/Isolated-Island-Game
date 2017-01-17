using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SendMessageHandler : PlayerOperationHandler
    {
        public SendMessageHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                throw new System.NotImplementedException();
            }
            else
            {
                return false;
            }
        }
    }
}
