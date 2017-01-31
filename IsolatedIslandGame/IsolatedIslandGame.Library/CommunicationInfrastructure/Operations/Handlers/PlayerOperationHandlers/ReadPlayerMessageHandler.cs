using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class ReadPlayerMessageHandler : PlayerOperationHandler
    {
        public ReadPlayerMessageHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int playerMessageID = (int)parameters[(byte)ReadPlayerMessageParameterCode.PlayerMessageID];

                return subject.User.CommunicationInterface.ReadPlayerMessage(subject.PlayerID, playerMessageID); ;
            }
            else
            {
                return false;
            }
        }
    }
}
