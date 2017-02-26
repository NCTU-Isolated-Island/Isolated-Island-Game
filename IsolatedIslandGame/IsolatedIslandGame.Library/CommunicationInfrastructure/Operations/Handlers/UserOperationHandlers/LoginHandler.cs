using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    class LoginHandler : UserOperationHandler
    {
        public LoginHandler(User subject) : base(subject, 2)
        {
        }

        internal override bool Handle(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                ulong facebookID = ulong.Parse((string)parameters[(byte)LoginParameterCode.FacebookID]);
                string accessToken = (string)parameters[(byte)LoginParameterCode.AccessToken];
                bool result = subject.CommunicationInterface.Login(facebookID, accessToken, out debugMessage, out errorCode);
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
                    SendResponse(operationCode, responseParameters);
                }
                else
                {
                    SendError(operationCode, errorCode, debugMessage);
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
