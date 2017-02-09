using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class DrawMaterialResponseHandler : ResponseHandler<Player, PlayerOperationCode>
    {
        public DrawMaterialResponseHandler(Player subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 2)
                        {
                            LogService.ErrorFormat(string.Format("DrawMaterialResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("DrawMaterialResponse Error DebugMessage: {0}", debugMessage);
                        subject.ResponseManager.DrawMaterialResponse(returnCode, 0, 0);
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
                    int itemID = (int)parameters[(byte)DrawMaterialResponseParameterCode.ItemID];
                    int itemCount = (int)parameters[(byte)DrawMaterialResponseParameterCode.ItemCount];
                    subject.ResponseManager.DrawMaterialResponse(returnCode, itemID, itemCount);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("DrawMaterialResponse Parameter Cast Error");
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
