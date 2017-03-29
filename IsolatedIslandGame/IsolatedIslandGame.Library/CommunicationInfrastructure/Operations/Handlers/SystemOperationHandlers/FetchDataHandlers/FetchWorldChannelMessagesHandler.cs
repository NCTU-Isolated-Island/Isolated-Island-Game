using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchWorldChannelMessagesHandler : SystemFetchDataHandler
    {
        public FetchWorldChannelMessagesHandler(SystemManager systemManager) : base(systemManager, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (WorldChannelMessage worldChannelMessage in SystemManager.Instance.GetWorldChannelMessages())
                    {
                        communicationInterface.User?.Player.SyncPlayerInformation(worldChannelMessage.Message.senderPlayerID);
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchWorldChannelMessagesResponseParameterCode.WorldChannelMessagesID, worldChannelMessage.WorldChannelMessageID },
                            { (byte)FetchWorldChannelMessagesResponseParameterCode.PlayerMessageID, worldChannelMessage.Message.playerMessageID },
                            { (byte)FetchWorldChannelMessagesResponseParameterCode.SenderPlayerID, worldChannelMessage.Message.senderPlayerID },
                            { (byte)FetchWorldChannelMessagesResponseParameterCode.SendTime, worldChannelMessage.Message.sendTime.ToBinary() },
                            { (byte)FetchWorldChannelMessagesResponseParameterCode.Content, worldChannelMessage.Message.content }
                        };
                        SendResponse(communicationInterface, fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchWorldChannelMessages Invalid Cast!");
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
