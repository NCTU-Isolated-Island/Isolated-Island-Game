using IsolatedIslandGame.Database.Repositories;
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
            string sqlString = $@"SELECT QuestRequirementRecordID, {DatabaseService.DatabaseName}_SettingData.QuestRequirementCollection.QuestRequirementID, {DatabaseService.DatabaseName}_SettingData.QuestRequirementCollection.QuestRequirementType
                from {DatabaseService.DatabaseName}_PlayerData.QuestRequirementRecordCollection, {DatabaseService.DatabaseName}_SettingData.QuestRequirementCollection
                WHERE QuestRecordID = @questRecordID AND {DatabaseService.DatabaseName}_PlayerData.QuestRequirementRecordCollection.QuestRequirementID = {DatabaseService.DatabaseName}_SettingData.QuestRequirementCollection.QuestRequirementID;";
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
                        case QuestRequirementType.CumulativeLoginSpecificDay:
                            if (SpecializeQuestRequirementRecordToCumulativeLoginSpecificDayQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.StayInSpecificOcean:
                            if (SpecializeQuestRequirementRecordToStayInSpecificOceanQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.MakeFriendSuccessfulSpecificNumberOfTime:
                            if (SpecializeQuestRequirementRecordToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.CloseDealSpecificNumberOfTime:
                            if (SpecializeQuestRequirementRecordToCloseDealSpecificNumberOfTimeQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.SendMaterialToIslandSpecificNumberOfTime:
                            if (SpecializeQuestRequirementRecordToSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.ExistedInSpecificNumberOcean:
                            if (SpecializeQuestRequirementRecordToExistedInSpecificNumberOceanQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOcean:
                            if (SpecializeQuestRequirementRecordToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOcean:
                            if (SpecializeQuestRequirementRecordToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.GetSpecificItem:
                            if (SpecializeQuestRequirementRecordToGetSpecificItemQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.CloseDealWithOutlander:
                            if (SpecializeQuestRequirementRecordToCloseDealWithOutlanderQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.CollectSpecificNumberBelongingGroupMaterial:
                            if (SpecializeQuestRequirementRecordToCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.SynthesizeSpecificScoreMaterial:
                            if (SpecializeQuestRequirementRecordToSynthesizeSpecificScoreMaterialQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.ScanSpecificQR_Code:
                            if (SpecializeQuestRequirementRecordToScanSpecificQR_CodeQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.HaveSpecificNumberFriend:
                            if (SpecializeQuestRequirementRecordToHaveSpecificNumberFriendQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.SynthesizeSuccessfulSpecificNumberOfTime:
                            if (SpecializeQuestRequirementRecordToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.HaveSpecificNumberKindMaterial:
                            if (SpecializeQuestRequirementRecordToHaveSpecificNumberKindMaterialQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.AddSpecificNumberDecorationToVessel:
                            if (SpecializeQuestRequirementRecordToAddSpecificNumberDecorationToVesselQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        case QuestRequirementType.HaveSpecificNumberDecorationOnVessel:
                            if (SpecializeQuestRequirementRecordToHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(info.questRequirementRecordID, requirement, out requirementRecord))
                            {
                                requirementRecords.Add(requirementRecord);
                            }
                            break;
                        default:
                            LogService.Fatal($"MySQL_QuestRecordRepository ListRequirementRecordsOfQuestRecord QuestRequirementType: {info.questRequirementType} not implemented");
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
                (QuestRecordID,QuestRequirementID) VALUES (@questRecordID,@questRequirementID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRecordID", questRecordID);
                command.Parameters.AddWithValue("questRequirementID", requirement.QuestRequirementID);
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
                case QuestRequirementType.CumulativeLoginSpecificDay:
                    {
                        return CreateCumulativeLoginSpecificDayQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.StayInSpecificOcean:
                    {
                        return CreateStayInSpecificOceanQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.MakeFriendSuccessfulSpecificNumberOfTime:
                    {
                        return CreateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.CloseDealSpecificNumberOfTime:
                    {
                        return CreateCloseDealSpecificNumberOfTimeQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.SendMaterialToIslandSpecificNumberOfTime:
                    {
                        return CreateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.ExistedInSpecificNumberOcean:
                    {
                        return CreateExistedInSpecificNumberOceanQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOcean:
                    {
                        return CreateSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOcean:
                    {
                        return CreateCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.GetSpecificItem:
                    {
                        return CreateGetSpecificItemQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.CloseDealWithOutlander:
                    {
                        return CreateCloseDealWithOutlanderQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.CollectSpecificNumberBelongingGroupMaterial:
                    {
                        return CreateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.SynthesizeSpecificScoreMaterial:
                    {
                        return CreateSynthesizeSpecificScoreMaterialQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.ScanSpecificQR_Code:
                    {
                        return CreateScanSpecificQR_CodeQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.HaveSpecificNumberFriend:
                    {
                        return CreateHaveSpecificNumberFriendQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.SynthesizeSuccessfulSpecificNumberOfTime:
                    {
                        return CreateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.HaveSpecificNumberKindMaterial:
                    {
                        return CreateHaveSpecificNumberKindMaterialQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.AddSpecificNumberDecorationToVessel:
                    {
                        return CreateAddSpecificNumberDecorationToVesselQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                case QuestRequirementType.HaveSpecificNumberDecorationOnVessel:
                    {
                        return CreateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(questRequirementRecordID, requirement, out requirementRecord);
                    }
                default:
                    requirementRecord = null;
                    LogService.Fatal($"MySQL_QuestRecordRepository CreateQuestRequirementRecord QuestRequirementType: {requirement.QuestRequirementType} not implemented");
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
        public override bool IsPlayerHasAnyStillNotGottenRewardQuest(int playerID, int questID)
        {
            string sqlString = @"SELECT QuestRecordID
                from QuestRecordCollection 
                WHERE PlayerID = @playerID AND QuestID = @questID AND HasGottenReward = false;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        #region create quest requirement record
        protected override bool CreateCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO CLSD_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasAchievedCumulativeLoginDayCount) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if(command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new CumulativeLoginSpecificDayQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO SISO_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasStayedInSpecificOcean) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new StayInSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO MFSSNOT_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,SuccessfulCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO CDSNOT_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,CloseDealCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new CloseDealSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO SMTISNOT_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,SendMaterialToIslandCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            requirementRecord = new ExistedInSpecificNumberOceanQuestRequirementRecord(requirementRecordID, requirement, new HashSet<OceanType>());
            return true;
        }
        protected override bool CreateSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            requirementRecord = new SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, new HashSet<int>());
            return true;
        }
        protected override bool CreateCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            requirementRecord = new CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, new HashSet<int>());
            return true;
        }
        protected override bool CreateGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO GSI_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasGottenSpecificItem) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new GetSpecificItemQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO CDWO_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasCloseDealWithOutlander) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new CloseDealWithOutlanderQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO CSNBGM_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,MaterialCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateSynthesizeSpecificScoreMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO SSSM_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasSynthesizedSpecificScoreMaterial) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new SynthesizeSpecificScoreMaterialQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO SSQRC_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,HasScannedCorrectQR_Code) VALUES (@requirementRecordID,false) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new ScanSpecificQR_CodeQuestRequirementRecord(requirementRecordID, requirement, false);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO HSNF_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,FriendCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new HaveSpecificNumberFriendQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO SSSNOT_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,SuccessfulCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO HSNKM_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,MaterialKindNumber) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new HaveSpecificNumberKindMaterialQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO ASNDTV_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,AddedDecorationCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new AddSpecificNumberDecorationToVesselQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        protected override bool CreateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"INSERT INTO HSNDOV_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,DecorationCount) VALUES (@requirementRecordID,0) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() > 0)
                {
                    requirementRecord = new HaveSpecificNumberDecorationOnVesselQuestRequirementRecord(requirementRecordID, requirement, 0);
                    return true;
                }
                else
                {
                    requirementRecord = null;
                    return false;
                }
            }
        }
        #endregion

        #region specialize quest requirement record
        protected override bool SpecializeQuestRequirementRecordToCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasAchievedCumulativeLoginDayCount
                from CLSD_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasAchievedCumulativeLoginDayCount = reader.GetBoolean(0);
                        requirementRecord = new CumulativeLoginSpecificDayQuestRequirementRecord(requirementRecordID, requirement, hasAchievedCumulativeLoginDayCount);
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
        protected override bool SpecializeQuestRequirementRecordToStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasStayedInSpecificOcean
                from SISO_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasStayedInSpecificOcean = reader.GetBoolean(0);
                        requirementRecord = new StayInSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, hasStayedInSpecificOcean);
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
        protected override bool SpecializeQuestRequirementRecordToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT SuccessfulCount
                from MFSSNOT_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int successfulCount = reader.GetInt32(0);
                        requirementRecord = new MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, successfulCount);
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
        protected override bool SpecializeQuestRequirementRecordToCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT CloseDealCount
                from CDSNOT_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int closeDealCount = reader.GetInt32(0);
                        requirementRecord = new CloseDealSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, closeDealCount);
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
        protected override bool SpecializeQuestRequirementRecordToSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT SendMaterialToIslandCount
                from SMTISNOT_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int sendMaterialToIslandCount = reader.GetInt32(0);
                        requirementRecord = new SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, sendMaterialToIslandCount);
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
        protected override bool SpecializeQuestRequirementRecordToExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            HashSet<OceanType> existedOceans = new HashSet<OceanType>();
            string sqlString = @"SELECT ExistedOcean
                from EISNO_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OceanType existedOcean = (OceanType)reader.GetByte(0);
                        existedOceans.Add(existedOcean);
                    }
                }
            }
            requirementRecord = new ExistedInSpecificNumberOceanQuestRequirementRecord(requirementRecordID, requirement, existedOceans);
            return true;
        }
        protected override bool SpecializeQuestRequirementRecordToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from SMTSNDOFITSSO_QuestRequirementRecordCollection 
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
            requirementRecord = new SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }
        protected override bool SpecializeQuestRequirementRecordToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from CDWSNDFITSSO_QuestRequirementRecordCollection 
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
            requirementRecord = new CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }
        protected override bool SpecializeQuestRequirementRecordToGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasGottenSpecificItem
                from GSI_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasGottenSpecificItem = reader.GetBoolean(0);
                        requirementRecord = new GetSpecificItemQuestRequirementRecord(requirementRecordID, requirement, hasGottenSpecificItem);
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
        protected override bool SpecializeQuestRequirementRecordToCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasCloseDealWithOutlander
                from CDWO_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasCloseDealWithOutlander = reader.GetBoolean(0);
                        requirementRecord = new CloseDealWithOutlanderQuestRequirementRecord(requirementRecordID, requirement, hasCloseDealWithOutlander);
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
        protected override bool SpecializeQuestRequirementRecordToCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT MaterialCount
                from CSNBGM_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialCount = reader.GetInt32(0);
                        requirementRecord = new CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(requirementRecordID, requirement, materialCount);
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
        protected override bool SpecializeQuestRequirementRecordToSynthesizeSpecificScoreMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasSynthesizedSpecificScoreMaterial
                from SSSM_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasSynthesizedSpecificScoreMaterial = reader.GetBoolean(0);
                        requirementRecord = new SynthesizeSpecificScoreMaterialQuestRequirementRecord(requirementRecordID, requirement, hasSynthesizedSpecificScoreMaterial);
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
        protected override bool SpecializeQuestRequirementRecordToScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT HasScannedCorrectQR_Code
                from SSQRC_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool hasScannedCorrectQR_Code = reader.GetBoolean(0);
                        requirementRecord = new ScanSpecificQR_CodeQuestRequirementRecord(requirementRecordID, requirement, hasScannedCorrectQR_Code);
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
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT FriendCount
                from HSNF_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int friendCount = reader.GetInt32(0);
                        requirementRecord = new HaveSpecificNumberFriendQuestRequirementRecord(requirementRecordID, requirement, friendCount);
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
        protected override bool SpecializeQuestRequirementRecordToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT SuccessfulCount
                from SSSNOT_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int successfulCount = reader.GetInt32(0);
                        requirementRecord = new SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(requirementRecordID, requirement, successfulCount);
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
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT MaterialKindNumber
                from HSNKM_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialKindNumber = reader.GetInt32(0);
                        requirementRecord = new HaveSpecificNumberKindMaterialQuestRequirementRecord(requirementRecordID, requirement, materialKindNumber);
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
        protected override bool SpecializeQuestRequirementRecordToAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT AddedDecorationCount
                from ASNDTV_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int addedDecorationCount = reader.GetInt32(0);
                        requirementRecord = new AddSpecificNumberDecorationToVesselQuestRequirementRecord(requirementRecordID, requirement, addedDecorationCount);
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
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT DecorationCount
                from HSNDOV_QuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int decorationCount = reader.GetInt32(0);
                        requirementRecord = new HaveSpecificNumberDecorationOnVesselQuestRequirementRecord(requirementRecordID, requirement, decorationCount);
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
        #endregion

        #region update quest requirement record
        public override void UpdateCumulativeLoginSpecificDayQuestRequirementRecord(CumulativeLoginSpecificDayQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE CLSD_QuestRequirementRecordCollection SET 
                HasAchievedCumulativeLoginDayCount = @hasAchievedCumulativeLoginDayCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasAchievedCumulativeLoginDayCount", record.HasAchievedCumulativeLoginDayCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateCumulativeLoginSpecificDayQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateStayInSpecificOceanQuestRequirementRecord(StayInSpecificOceanQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE SISO_QuestRequirementRecordCollection SET 
                HasStayedInSpecificOcean = @hasStayedInSpecificOcean
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasStayedInSpecificOcean", record.HasStayedInSpecificOcean);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateStayInSpecificOceanQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE MFSSNOT_QuestRequirementRecordCollection SET 
                SuccessfulCount = @successfulCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("successfulCount", record.SuccessfulCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(CloseDealSpecificNumberOfTimeQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE CDSNOT_QuestRequirementRecordCollection SET 
                CloseDealCount = @closeDealCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("closeDealCount", record.CloseDealCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE SMTISNOT_QuestRequirementRecordCollection SET 
                SendMaterialToIslandCount = @sendMaterialToIslandCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("sendMaterialToIslandCount", record.SendMaterialToIslandCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override bool AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(ExistedInSpecificNumberOceanQuestRequirementRecord record, OceanType locatedOceanType)
        {
            string sqlString = @"INSERT INTO EISNO_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,ExistedOcean) VALUES (@questRequirementRecordID,@existedOcean) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);
                command.Parameters.AddWithValue("existedOcean", (byte)locatedOceanType);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public override bool AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {
            string sqlString = @"INSERT INTO SMTSNDOFITSSO_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@questRequirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", theOtherPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public override bool AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {
            string sqlString = @"INSERT INTO CDWSNDFITSSO_QuestRequirementRecordCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@questRequirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", theOtherPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public override void UpdateGetSpecificItemQuestRequirementRecord(GetSpecificItemQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE GSI_QuestRequirementRecordCollection SET 
                HasGottenSpecificItem = @hasGottenSpecificItem
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasGottenSpecificItem", record.HasGottenSpecificItem);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateGetSpecificItemQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateCloseDealWithOutlanderQuestRequirementRecord(CloseDealWithOutlanderQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE CDWO_QuestRequirementRecordCollection SET 
                HasCloseDealWithOutlander = @hasCloseDealWithOutlander
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasCloseDealWithOutlander", record.HasCloseDealWithOutlander);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateCloseDealWithOutlanderQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE CSNBGM_QuestRequirementRecordCollection SET 
                MaterialCount = @materialCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("materialCount", record.MaterialCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord(SynthesizeSpecificScoreMaterialQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE SSSM_QuestRequirementRecordCollection SET 
                HasSynthesizedSpecificScoreMaterial = @hasSynthesizedSpecificScoreMaterial
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasSynthesizedSpecificScoreMaterial", record.HasSynthesizedSpecificScoreMaterial);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateScanSpecificQR_CodeQuestRequirementRecord(ScanSpecificQR_CodeQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE SSQRC_QuestRequirementRecordCollection SET 
                HasScannedCorrectQR_Code = @hasScannedCorrectQR_Code
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("hasScannedCorrectQR_Code", record.HasScannedCorrectQR_Code);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateScanSpecificQR_CodeQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateHaveSpecificNumberFriendQuestRequirementRecord(HaveSpecificNumberFriendQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE HSNF_QuestRequirementRecordCollection SET 
                FriendCount = @friendCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("friendCount", record.FriendCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateHaveSpecificNumberFriendQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE SSSNOT_QuestRequirementRecordCollection SET 
                SuccessfulCount = @successfulCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("successfulCount", record.SuccessfulCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(HaveSpecificNumberKindMaterialQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE HSNKM_QuestRequirementRecordCollection SET 
                MaterialKindNumber = @materialKindNumber
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("materialKindNumber", record.MaterialKindNumber);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(AddSpecificNumberDecorationToVesselQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE ASNDTV_QuestRequirementRecordCollection SET 
                AddedDecorationCount = @addedDecorationCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("addedDecorationCount", record.AddedDecorationCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        public override void UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(HaveSpecificNumberDecorationOnVesselQuestRequirementRecord record)
        {
            string sqlString = @"UPDATE HSNDOV_QuestRequirementRecordCollection SET 
                DecorationCount = @decorationCount
                WHERE QuestRequirementRecordID = @questRequirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("decorationCount", record.DecorationCount);
                command.Parameters.AddWithValue("questRequirementRecordID", record.QuestRequirementRecordID);

                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord Error QuestRequirementRecordID: {record.QuestRequirementRecordID}");
                }
            }
        }
        #endregion
    }
}
