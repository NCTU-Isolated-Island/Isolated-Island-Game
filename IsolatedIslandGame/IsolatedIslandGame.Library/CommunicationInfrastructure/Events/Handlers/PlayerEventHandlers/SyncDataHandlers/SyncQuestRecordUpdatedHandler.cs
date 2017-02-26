using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncQuestRecordUpdatedHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncQuestRecordUpdatedHandler(Player subject) : base(subject, 1)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    byte[] questRecordDataByteArray = (byte[])parameters[(byte)SyncQuestRecordUpdatedParameterCode.QuestRecordDataByteArray];
                    QuestRecord questRecord = SerializationHelper.QuestRecordDeserialize(questRecordDataByteArray);

                    QuestManager.Instance.AddQuest(questRecord.Quest);
                    subject.AddQuestRecord(questRecord);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncQuestRecordUpdated Parameter Cast Error");
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
