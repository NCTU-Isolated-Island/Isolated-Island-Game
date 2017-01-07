using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class CreateCharacterHandler : PlayerOperationHandler
    {
        public CreateCharacterHandler(Player subject) : base(subject, 3)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                string nickname = (string)parameters[(byte)CreateCharacterParameterCode.Nickname];
                string signature = (string)parameters[(byte)CreateCharacterParameterCode.Signature];
                GroupType groupType = (GroupType)parameters[(byte)CreateCharacterParameterCode.GroupType];
                if(subject.GroupType != GroupType.No)
                {
                    errorCode = ErrorCode.AlreadyExisted;
                    debugMessage = "already created character";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat("Player: {0}, CreateCharacter Fail because GroupType is not No, which is {1}", subject.IdentityInformation, subject.GroupType);
                    return false;
                }
                if (groupType == GroupType.No || !Enum.IsDefined(typeof(GroupType), groupType))
                {
                    errorCode = ErrorCode.ParameterError;
                    debugMessage = "invalid group type";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat("Player: {0}, CreateCharacter Fail because GroupType is invalid, GroupType is {1}", subject.IdentityInformation, groupType);
                    return false;
                }
                else
                {
                    subject.CreateCharacter(nickname, signature, groupType);
                    Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                    {
                        { (byte)CreateCharacterResponseParameterCode.Nickname, subject.Nickname },
                        { (byte)CreateCharacterResponseParameterCode.Signature, subject.Signature },
                        { (byte)CreateCharacterResponseParameterCode.GroupType, subject.GroupType }
                    };
                    SendResponse(operationCode, responseParameters);
                    LogService.InfoFormat("Player: {0}, CreateCharacter, Nickname: {1}, Signature: {2}, GroupType: {3}", subject.IdentityInformation, subject.Nickname, subject.Signature, subject.GroupType);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
