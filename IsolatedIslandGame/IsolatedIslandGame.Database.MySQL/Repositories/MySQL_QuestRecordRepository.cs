﻿using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.Quests.RequirementRecords;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_QuestRecordRepository : QuestRecordRepository
    {
        public override bool CreateQuestRecord(int playerID, Quest quest, out QuestRecord questRecord)
        {
            int questRecordID = 0;
            string sqlString = @"INSERT INTO QuestRecordCollection 
                (PlayerID,QuestID) VALUES (@playerID,@questID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
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
                if(requirement.CreateRequirementRecord(questRecordID, playerID, out requirementRecord))
                {
                    requirementRecords.Add(requirementRecord);
                }
            }
            questRecord = new QuestRecord(questRecordID, playerID, quest, requirementRecords, false);
            return true;
        }
        public override List<QuestRecord> ListOfPlayer(int playerID)
        {
            List<QuestRecordInfo> infos = new List<QuestRecordInfo>();
            string sqlString = @"SELECT QuestRecordID, QuestID, HasGottenReward
                from QuestRecordCollection 
                WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRecordID = reader.GetInt32(0);
                        int questID = reader.GetInt32(1);
                        bool hasGottenReward = reader.GetBoolean(2);

                        infos.Add(new QuestRecordInfo
                        {
                            questRecordID = questRecordID,
                            questID = questID,
                            hasGottenReward = hasGottenReward
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
                    QuestRecord record = new QuestRecord(info.questRecordID, playerID, quest, ListRequirementRecordsOfQuestRecord(info.questRecordID, playerID), info.hasGottenReward);
                    records.Add(record);
                }
            }
            return records;
        }
        protected override List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(int questRecordID, int playerID)
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

            foreach (var info in infos)
            {
                QuestRequirement requirement;
                if (QuestManager.Instance.FindQuestRequirement(info.questRequirementID, out requirement))
                {
                    QuestRequirementRecord requirementRecord;
                    switch (info.questRequirementType)
                    {
                        case QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameOcean:
                            if (SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendTheSameOceanRequirementRecord(info.questRequirementRecordID, requirement, playerID, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                    }
                }
            }
            return requirementRecords;
        }
        public override bool CreateQuestRequirementRecord(int questRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            int questRequirementRecordID;
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
                        questRequirementRecordID = reader.GetInt32(0);
                    }
                    else
                    {
                        requirementRecord = null;
                        return false;
                    }
                }
            }
            switch (requirement.QuestRequirementType)
            {
                case QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameOcean:
                    {
                        requirementRecord = new SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(questRequirementRecordID, requirement, new HashSet<int>());
                    }
                    return true;
                case QuestRequirementType.CloseDealWithDifferentFriendInTheSameOcean:
                    {
                        requirementRecord = new CloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(questRequirementRecordID, requirement, new HashSet<int>());
                    }
                    return true;
                default:
                    requirementRecord = null;
                    return false;
            }
        }
        public override bool MarkQuestRecordHasGottenReward(int questRecordID)
        {
            string sqlString = @"UPDATE QuestRecordCollection SET 
                HasGottenReward = true
                WHERE QuestRecordID = @questRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRecordID", questRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository MarkQuestRecordHasGottenReward Error QuestRecordID: {questRecordID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            string sqlString = @"INSERT INTO SMTDOFITSO_QuestRequirementRecordPlayerIDCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@requirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", friendPlayerID);
                if(command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord Error RequirementRecordID: {requirementRecordID}, FriendPlayerID: {friendPlayerID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendTheSameOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from SMTDOFITSO_QuestRequirementRecordPlayerIDCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);

                        friendPlayerIDs.Add(friendPlayerID);
                    }
                }
            }
            requirementRecord = new SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }

        public override bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            string sqlString = @"INSERT INTO CDWDFITSO_QuestRequirementRecordPlayerIDCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@requirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", friendPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord Error RequirementRecordID: {requirementRecordID}, FriendPlayerID: {friendPlayerID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToCloseDealWithDifferentFriendInTheSameOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from CDWDFITSO_QuestRequirementRecordPlayerIDCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);

                        friendPlayerIDs.Add(friendPlayerID);
                    }
                }
            }
            requirementRecord = new CloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }
    }
}
