using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;
using System;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SendMaterialToIslandHandler : PlayerOperationHandler
    {
        public SendMaterialToIslandHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int materialItemID = (int)parameters[(byte)SendMaterialToIslandParameterCode.MaterialItemID];
                throw new NotImplementedException("SendMaterialToIslandHandler Handle");
            }
            else
            {
                return false;
            }
        }
    }
}
