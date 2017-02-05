using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

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
                if(requirement.CreateRequirementRecord(player, out requirementRecord))
                {
                    requirementRecords.Add(requirementRecord);
                }
            }
            questRecord = new QuestRecord(questRecordID, player.PlayerID, quest, requirementRecords);
            return true;
        }

        public override bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord)
        {
            throw new NotImplementedException();
        }

        public override List<QuestRecord> ListOfPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        protected override List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(QuestRecord questRecord, Player player)
        {
            throw new NotImplementedException();
        }

        protected override bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendRequirementRecord(int requirementRecordID, QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord)
        {
            throw new NotImplementedException();
        }
    }
}
