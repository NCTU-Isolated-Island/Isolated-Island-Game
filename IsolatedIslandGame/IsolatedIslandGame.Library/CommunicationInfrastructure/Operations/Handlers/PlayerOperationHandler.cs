using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public abstract class PlayerOperationHandler : OperationHandler<Player, PlayerOperationCode>
    {
        public PlayerOperationHandler(Player subject, int correctParameterCount) : base(subject, correctParameterCount)
        {
        }

        internal override void SendError(PlayerOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            base.SendError(operationCode, errorCode, debugMessage);

            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            subject.ResponseManager.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
        internal override void SendResponse(PlayerOperationCode operationCode, Dictionary<byte, object> parameter)
        {
            subject.ResponseManager.SendResponse(operationCode, ErrorCode.NoError, null, parameter);
        }
    }
}
