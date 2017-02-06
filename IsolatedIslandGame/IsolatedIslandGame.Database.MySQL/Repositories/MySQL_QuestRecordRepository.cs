using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_QuestRecordRepository : QuestRecordRepository
    {
        public override bool CreateQuestRecord(Player player, Quest quest, out QuestRecord questRecord)
        {
            int questRecordID = 0;
            string sqlString = @"INSERT INTO QuestRecordCollection 
                (PlayerID,QuestID) VALUES (@playerID,@questID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", player.PlayerID);
                command.Parameters.AddWithValue("questID", quest.QuestID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        questRecordID = reader.GetInt32(0);
                    }
                    else
                    {
                        questRecord = null;
                        return false;
                    }
                }
            }
            List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();
            foreach(var requirement in quest.Requirements)
            {
                QuestRequirementRecord requirementRecord;
                if(requirement.CreateRequirementRecord(questRecordID, player, out requirementRecord))
                {
                    requirementRecords.Add(requirementRecord);
                }
            }
            questRecord = new QuestRecord(questRecordID, player.PlayerID, quest, requirementRecords);
            return true;
        }

        public override bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(int questRecordID, Player player, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO QuestRequirementRecordCollection 
                (QuestRecordID,QuestRequirementID,QuestRequirementType) VALUES (@questRecordID,@questRequirementID,@questRequirementType) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRecordID", questRecordID);
                command.Parameters.AddWithValue("questRequirementID", requirement.QuestRequirementID);
                command.Parameters.AddWithValue("questRequirementType", (byte)requirement.QuestRequirementType);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int questRequirementRecordID = reader.GetInt32(0);
                        requirementRecord = new SendMessageToDifferentOnlineFriendQuestRequirementRecord(questRequirementRecordID, player, requirement, new HashSet<int>());
                        return true;
                    }
                    else
                    {
                        requirementRecord = null;
                        return false;
                    }
                }
            }
        }

        public override List<QuestRecord> ListOfPlayer(Player player)
        {
            List<QuestRecordInfo> infos = new List<QuestRecordInfo>();
            string sqlString = @"SELECT QuestRecordID, QuestID
                from QuestRecordCollection 
                WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", player.PlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRecordID = reader.GetInt32(0);
                        int questID = reader.GetInt32(1);

                        infos.Add(new QuestRecordInfo
                        {
                            questRecordID = questRecordID,
                            questID = questID
                        });
                    }
                }
            }
            List<QuestRecord> records = new List<QuestRecord>();

            foreach (var info in infos)
            {
                Quest quest;
                if (QuestManager.Instance.FindQuest(info.questID, out quest))
                {
                    QuestRecord record = new QuestRecord(info.questRecordID, player.PlayerID, quest, ListRequirementRecordsOfQuestRecord(info.questRecordID, player));
                    records.Add(record);
                }
            }
            return records;
        }

        protected override List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(int questRecordID, Player player)
        {
            List<QuestRequirementRecordInfo> infos = new List<QuestRequirementRecordInfo>();
            string sqlString = @"SELECT QuestRequirementRecordID, QuestRequirementID, QuestRequirementType
                from QuestRequirementRecordCollection 
                WHERE QuestRecordID = @questRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRecordID", questRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRequirementRecordID = reader.GetInt32(0);
                        int questRequirementID = reader.GetInt32(1);
                        QuestRequirementType questRequirementType = (QuestRequirementType)reader.GetByte(2);

                        infos.Add(new QuestRequirementRecordInfo
                        {
                            questRequirementRecordID = questRequirementRecordID,
                            questRequirementID = questRequirementID,
                            questRequirementType = questRequirementType
                        });
                    }
                }
            }
            List<QuestRequirementRecord> requirementRecords = new List<QuestRequirementRecord>();

            foreach(var info in infos)
            {
                QuestRequirement requirement;
                if(QuestManager.Instance.FindQuestRequirement(info.questRequirementID, out requirement))
                {
                    QuestRequirementRecord requirementRecord;
                    switch (info.questRequirementType)
                    {
                        case QuestRequirementType.SendMessageToDifferentOnlineFriend:
                            if (SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendRequirementRecord(info.questRequirementRecordID, requirement, player, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                    }
                }
            }
            return requirementRecords;
        }
        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord(int requirementRecordID, int onlineFriendPlayerID)
        {
            string sqlString = @"INSERT INTO SendMessageToDifferentOnlineFriendQstReqRecordPlayerIDCollection 
                (QuestRequirementRecordID,OnlineFriendPlayerID) VALUES (@requirementRecordID,@onlineFriendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                command.Parameters.AddWithValue("onlineFriendPlayerID", onlineFriendPlayerID);
                if(command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord Error RequirementRecordID: {requirementRecordID}, OnlineFriendPlayerID: {onlineFriendPlayerID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendRequirementRecord(int requirementRecordID, QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> onlineFriendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT OnlineFriendPlayerID
                from SendMessageToDifferentOnlineFriendQstReqRecordPlayerIDCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int onlineFriendPlayerID = reader.GetInt32(0);

                        onlineFriendPlayerIDs.Add(onlineFriendPlayerID);
                    }
                }
            }
            requirementRecord = new SendMessageToDifferentOnlineFriendQuestRequirementRecord(requirementRecordID, player, requirement, onlineFriendPlayerIDs);
            return true;
        }
    }
}
