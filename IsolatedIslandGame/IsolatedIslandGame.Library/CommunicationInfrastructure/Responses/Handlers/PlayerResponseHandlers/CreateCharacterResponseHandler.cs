using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class CreateCharacterResponseHandler : ResponseHandler<Player, PlayerOperationCode>
    {
        public CreateCharacterResponseHandler(Player subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 3)
                        {
                            LogService.ErrorFormat(string.Format("CreateCharacterResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case ErrorCode.ParameterError:
                    {
                        LogService.ErrorFormat("CreateCharacter Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "未知的陣營");
                        return false;
                    }
                default:
                    {
                        LogService.ErrorFormat("CreateCharacter Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "未知的錯誤種類");
                        return false;
                    }
            }
        }
        internal override bool Handle(PlayerOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                try
                {
                    string nickname = (string)parameters[(byte)CreateCharacterResponseParameterCode.Nickname];
                    string signature = (string)parameters[(byte)CreateCharacterResponseParameterCode.Signature];
                    GroupType groupType = (GroupType)parameters[(byte)CreateCharacterResponseParameterCode.GroupType];
                    subject.CreateCharacter(nickname, signature, groupType);
                    subject.OperationManager.FetchDataResolver.FetchVessel();
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("CreateCharacterResponse Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
