using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    public class ScanQR_CodeHandler : PlayerOperationHandler
    {
        public ScanQR_CodeHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string qrCodeString = (string)parameters[(byte)ScanQR_CodeParameterCode.QR_CodeString];

                subject.ScanQR_Code(qrCodeString);
                SendResponse(operationCode, new Dictionary<byte, object>());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
