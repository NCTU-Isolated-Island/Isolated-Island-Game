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

        #region create quest requirement record
        protected override bool CreateCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateSynthesizeSpecificScoreMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool CreateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        #endregion

        #region specialize quest requirement record
        protected override bool SpecializeQuestRequirementRecordToCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToSynthesizeSpecificScoreMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        protected override bool SpecializeQuestRequirementRecordToHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord)
        {

        }
        #endregion

        #region update quest requirement record
        public override void UpdateCumulativeLoginSpecificDayQuestRequirementRecord(CumulativeLoginSpecificDayQuestRequirementRecord record)
        {

        }
        public override void UpdateStayInSpecificOceanQuestRequirementRecord(StayInSpecificOceanQuestRequirementRecord record)
        {

        }
        public override void UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {

        }
        public override void UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(CloseDealSpecificNumberOfTimeQuestRequirementRecord record)
        {

        }
        public override void UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord record)
        {

        }
        public override bool AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(ExistedInSpecificNumberOceanQuestRequirementRecord record, OceanType locatedOceanType)
        {

        }
        public override bool AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {

        }
        public override bool AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {

        }
        public override void UpdateGetSpecificItemQuestRequirementRecord(GetSpecificItemQuestRequirementRecord record)
        {

        }
        public override void UpdateCloseDealWithOutlanderQuestRequirementRecord(CloseDealWithOutlanderQuestRequirementRecord record)
        {

        }
        public override void UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord record)
        {

        }
        public override void UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord(SynthesizeSpecificScoreMaterialQuestRequirementRecord record)
        {

        }
        public override void UpdateScanSpecificQR_CodeQuestRequirementRecord(ScanSpecificQR_CodeQuestRequirementRecord record)
        {

        }
        public override void UpdateHaveSpecificNumberFriendQuestRequirementRecord(HaveSpecificNumberFriendQuestRequirementRecord record)
        {

        }
        public override void UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {

        }
        public override void UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(HaveSpecificNumberKindMaterialQuestRequirementRecord record)
        {

        }
        public override void UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(AddSpecificNumberDecorationToVesselQuestRequirementRecord record)
        {

        }
        public override void UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(HaveSpecificNumberDecorationOnVesselQuestRequirementRecord record)
        {

        }
        #endregion

        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            string sqlString = @"INSERT INTO SMTDOFITSSO_QuestRequirementRecordPlayerIDCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@requirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", friendPlayerID);
                if(command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord Error RequirementRecordID: {requirementRecordID}, FriendPlayerID: {friendPlayerID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendTheSameSpecificOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from SMTDOFITSSO_QuestRequirementRecordPlayerIDCollection 
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
            requirementRecord = new SendMessageToDifferentOnlineFriendTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }

        public override bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            string sqlString = @"INSERT INTO CDWDFITSSO_QuestRequirementRecordPlayerIDCollection 
                (QuestRequirementRecordID,FriendPlayerID) VALUES (@requirementRecordID,@friendPlayerID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                command.Parameters.AddWithValue("friendPlayerID", friendPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository AddPlayerIDToCloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord Error RequirementRecordID: {requirementRecordID}, FriendPlayerID: {friendPlayerID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToCloseDealWithDifferentFriendInTheSameSpecificOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            HashSet<int> friendPlayerIDs = new HashSet<int>();
            string sqlString = @"SELECT FriendPlayerID
                from CDWDFITSSO_QuestRequirementRecordPlayerIDCollection 
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
            requirementRecord = new CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, requirement, friendPlayerIDs);
            return true;
        }

        public override bool MarkScanQR_CodeQuestRequirementRecordHasScannedCorrectQR_Code(int requirementRecordID)
        {
            string sqlString = @"INSERT INTO HasScannedCorrectQR_CodeQuestRequirementRecordCollection 
                (QuestRequirementRecordID) VALUES (@requirementRecordID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_QuestRecordRepository SpecializeRequirementRecordToScanQR_CodeRequirementRecord Error RequirementRecordID: {requirementRecordID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override bool SpecializeRequirementRecordToScanQR_CodeRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            string sqlString = @"SELECT QuestRequirementRecordID
                from HasScannedCorrectQR_CodeQuestRequirementRecordCollection 
                WHERE QuestRequirementRecordID = @requirementRecordID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementRecordID", requirementRecordID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        requirementRecord = new ScanQR_CodeQuestRequirementRecord(requirementRecordID, requirement, true);
                        return true;
                    }
                    else
                    {
                        requirementRecord = new ScanQR_CodeQuestRequirementRecord(requirementRecordID, requirement, false);
                        return true;
                    }
                }
            }
            
        }

        protected override bool SpecializeRequirementRecordToCumulativeLoginRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord)
        {
            requirementRecord = new CumulativeLoginQuestRequirementRecord(requirementRecordID, requirement, false);
            return true;
        }
    }
}
