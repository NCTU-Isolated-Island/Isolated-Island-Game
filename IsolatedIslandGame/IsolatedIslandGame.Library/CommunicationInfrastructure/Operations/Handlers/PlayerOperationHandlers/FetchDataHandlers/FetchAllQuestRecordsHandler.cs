using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchAllQuestRecordsHandler : PlayerFetchDataHandler
    {
        public FetchAllQuestRecordsHandler(Player subject) : base(subject, 0)
        {
        }
        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, parameters))
            {
                try
                {
                    foreach (var questRecord in subject.QuestRecords)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllQuestRecordsResponseParameterCode.QuestRecordDataByteArray, SerializationHelper.TypeSerialize(questRecord) }
                        };
                        SendResponse(fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllQuestRecords Invalid Cast!");
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
