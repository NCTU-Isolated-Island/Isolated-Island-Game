using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class UseBlueprintResponseHandler : ResponseHandler<Player, PlayerOperationCode>
    {
        public UseBlueprintResponseHandler(Player subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 1)
                        {
                            LogService.ErrorFormat(string.Format("UseBlueprintResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case ErrorCode.ParameterError:
                    {
                        LogService.ErrorFormat("UseBlueprint Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "藍圖不存在");
                        return false;
                    }
                case ErrorCode.Fail:
                    {
                        LogService.ErrorFormat("UseBlueprint Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "素材不足");
                        return false;
                    }
                default:
                    {
                        LogService.ErrorFormat("UseBlueprint Error DebugMessage: {0}", debugMessage);
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
                    int blueprintID = (int)parameters[(byte)UseBlueprintResponseParameterCode.BlueprintID];
                    Blueprint blueprint;
                    if(BlueprintManager.Instance.FindBlueprint(blueprintID, out blueprint))
                    {
                        subject.TriggerUseBlueprintEvents(blueprint);
                        return true;
                    }
                    else
                    {
                        LogService.Error("UseBlueprintResponse Error Blueprint Not Existed");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("UseBlueprintResponse Parameter Cast Error");
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
