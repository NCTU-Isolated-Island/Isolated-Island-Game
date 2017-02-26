using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    class PlayerIDLoginHandler : UserOperationHandler
    {
        public PlayerIDLoginHandler(User subject) : base(subject, 2)
        {
        }

        internal override bool Handle(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int playerID = (int)parameters[(byte)PlayerIDLoginParameterCode.PlayerID];
                string password = (string)parameters[(byte)PlayerIDLoginParameterCode.Password];
                bool result = subject.CommunicationInterface.PlayerIDLogin(playerID, password, out debugMessage, out errorCode);
                if (result)
                {
                    Player player = subject.Player;
                    Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                    {
                        { (byte)LoginResponseParameterCode.PlayerID, player.PlayerID },
                        { (byte)LoginResponseParameterCode.FacebookID, player.FacebookID.ToString() },
                        { (byte)LoginResponseParameterCode.Nickname, player.Nickname },
                        { (byte)LoginResponseParameterCode.Signature, player.Signature },
                        { (byte)LoginResponseParameterCode.GroupType, (byte)player.GroupType },
                        { (byte)LoginResponseParameterCode.LastConnectedIPAddress, player.LastConnectedIPAddress.ToString() },
                        { (byte)LoginResponseParameterCode.NextDrawMaterialTime, player.NextDrawMaterialTime.ToBinary() },
                        { (byte)LoginResponseParameterCode.CumulativeLoginCount, player.CumulativeLoginCount },
                    };
                    SendResponse(UserOperationCode.Login, responseParameters);
                }
                else
                {
                    SendError(UserOperationCode.Login, errorCode, debugMessage);
                }
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}
