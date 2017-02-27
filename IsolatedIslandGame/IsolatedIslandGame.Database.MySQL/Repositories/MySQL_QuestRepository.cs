﻿using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.Quests.Requirements;
using IsolatedIslandGame.Library.Quests.Rewards;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_QuestRepository : QuestRepository
    {
        public override List<Quest> ListAll()
        {
            List<QuestInfo> questInfos = new List<QuestInfo>();
            string sqlString = @"SELECT QuestID, QuestType, QuestName, QuestDescription from QuestCollection ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questID = reader.GetInt32(0);
                        QuestType questType = (QuestType)reader.GetByte(1);
                        string questName = reader.GetString(2);
                        string questDescription = reader.GetString(3);
                        questInfos.Add(new QuestInfo
                        {
                            questID = questID,
                            questType = questType,
                            questName = questName,
                            questDescription = questDescription
                        });
                    }
                }
            }
            List<Quest> quests = new List<Quest>();
            foreach (QuestInfo questInfo in questInfos)
            {
                List<QuestRequirement> requirements = ListRequirementsOfQuest(questInfo.questID);
                List<QuestReward> rewards = ListRewardsOfQuest(questInfo.questID);
                quests.Add(new Quest(questInfo.questID, questInfo.questType, questInfo.questName, requirements, rewards, questInfo.questDescription));
            }
            return quests;
        }

        protected override List<QuestRequirement> ListRequirementsOfQuest(int questID)
        {
            List<QuestRequirementInfo> requirementInfos = new List<QuestRequirementInfo>();
            string sqlString = @"SELECT QuestRequirementID, QuestRequirementType from QuestRequirementCollection 
                WHERE QuestID = @questID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questID", questID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRequirementID = reader.GetInt32(0);
                        QuestRequirementType questRequirementType = (QuestRequirementType)reader.GetByte(1);

                        requirementInfos.Add(new QuestRequirementInfo
                        {
                            questRequirementID = questRequirementID,
                            questRequirementType = questRequirementType
                        });
                    }
                }
            }
            List<QuestRequirement> requirements = new List<QuestRequirement>();
            foreach(var info in requirementInfos)
            {
                QuestRequirement requirement;
                switch(info.questRequirementType)
                {
                    case QuestRequirementType.CumulativeLoginSpecificDay:
                        if (SpecializeQuestRequirementToCumulativeLoginSpecificDayQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.StayInSpecificOcean:
                        if (SpecializeQuestRequirementToStayInSpecificOceanQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.MakeFriendSuccessfulSpecificNumberOfTime:
                        if (SpecializeQuestRequirementToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CloseDealSpecificNumberOfTime:
                        if (SpecializeQuestRequirementToCloseDealSpecificNumberOfTimeQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.SendMaterialToIslandSpecificNumberOfTime:
                        if (SpecializeQuestRequirementToSendMaterialToIslandSpecificNumberOfTimeQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.ExistedInSpecificNumberOcean:
                        if (SpecializeQuestRequirementToExistedInSpecificNumberOceanQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOcean:
                        if (SpecializeQuestRequirementToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOcean:
                        if (SpecializeQuestRequirementToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.GetSpecificItem:
                        if (SpecializeQuestRequirementToGetSpecificItemQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CloseDealWithOutlander:
                        if (SpecializeQuestRequirementToCloseDealWithOutlanderQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CollectSpecificNumberBelongingGroupMaterial:
                        if (SpecializeQuestRequirementToCollectSpecificNumberBelongingGroupMaterialQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.SynthesizeSpecificScoreMaterial:
                        if (SpecializeQuestRequirementToSynthesizeSpecificScoreMaterialQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.ScanSpecificQR_Code:
                        if (SpecializeQuestRequirementToScanSpecificQR_CodeQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.HaveSpecificNumberFriend:
                        if (SpecializeQuestRequirementToHaveSpecificNumberFriendQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.SynthesizeSuccessfulSpecificNumberOfTime:
                        if (SpecializeQuestRequirementToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.HaveSpecificNumberKindMaterial:
                        if (SpecializeQuestRequirementToHaveSpecificNumberKindMaterialQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.AddSpecificNumberDecorationToVessel:
                        if (SpecializeQuestRequirementToAddSpecificNumberDecorationToVesselQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.HaveSpecificNumberDecorationOnVessel:
                        if (SpecializeQuestRequirementToHaveSpecificNumberDecorationOnVesselQuestRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    default:
                        LogService.Fatal($"MySQL_QuestRepository ListRequirementsOfQuest QuestRequirementType: {info.questRequirementType} not implemented");
                        break;
                }
            }
            return requirements;
        }

        protected override List<QuestReward> ListRewardsOfQuest(int questID)
        {
            List<QuestRewardInfo> rewardInfos = new List<QuestRewardInfo>();
            string sqlString = @"SELECT QuestRewardID, QuestRewardType from QuestRewardCollection 
                WHERE QuestID = @questID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questID", questID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRewardID = reader.GetInt32(0);
                        QuestRewardType questRewardType = (QuestRewardType)reader.GetByte(1);

                        rewardInfos.Add(new QuestRewardInfo
                        {
                            questRewardID = questRewardID,
                            questRewardType = questRewardType
                        });
                    }
                }
            }
            List<QuestReward> rewards = new List<QuestReward>();
            foreach (var info in rewardInfos)
            {
                QuestReward reward;
                switch(info.questRewardType)
                {
                    case QuestRewardType.GiveSpecificNumberSpecificItem:
                        if (SpecializeQuestRewardToGiveSpecificNumberSpecificItemQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.UnlockSpecificBlueprint:
                        if (SpecializeQuestRewardToUnlockSpecificBlueprintQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.AcceptSpecificQuest:
                        if (SpecializeQuestRewardToAcceptSpecificQuestQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.GiveSpecificNumberSpecificScoreRandomMaterial:
                        if (SpecializeQuestRewardToGiveSpecificNumberSpecificScoreRandomMaterialQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterial:
                        if (SpecializeQuestRewardToGiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterial:
                        if (SpecializeQuestRewardToGiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    default:
                        LogService.Fatal($"MySQL_QuestRepository ListRewardsOfQuest QuestRewardType: {info.questRewardType} not implemented");
                        break;
                }
            }
            return rewards;
        }

        #region Specialize QuestRequirement
        protected override bool SpecializeQuestRequirementToCumulativeLoginSpecificDayQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificDayCount
                from CLSD_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificDayCount = reader.GetInt32(0);
                        requirement = new CumulativeLoginSpecificDayQuestRequirement(requirementID, specificDayCount);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToStayInSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanType
                from SISO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OceanType SpecificOceanType = (OceanType)reader.GetByte(0);
                        requirement = new StayInSpecificOceanQuestRequirement(requirementID, SpecificOceanType);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificNumberOfTime
                from MFSSNOT_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificNumberOfTime = reader.GetInt32(0);
                        requirement = new MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement(requirementID, specificNumberOfTime);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToCloseDealSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificNumberOfTime
                from CDSNOT_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificNumberOfTime = reader.GetInt32(0);
                        requirement = new CloseDealSpecificNumberOfTimeQuestRequirement(requirementID, specificNumberOfTime);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToSendMaterialToIslandSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificNumberOfTime
                from SMTISNOT_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificNumberOfTime = reader.GetInt32(0);
                        requirement = new SendMaterialToIslandSpecificNumberOfTimeQuestRequirement(requirementID, specificNumberOfTime);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToExistedInSpecificNumberOceanQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanNumber
                from EISNO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificOceanNumber = reader.GetInt32(0);
                        requirement = new ExistedInSpecificNumberOceanQuestRequirement(requirementID, specificOceanNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanType, SpecificFriendNumber
                from SMTSNDOFITSSO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OceanType specificOceanType = (OceanType)reader.GetByte(0);
                        int specificFriendNumber = reader.GetInt32(1);
                        requirement = new SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(requirementID, specificOceanType, specificFriendNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanType, SpecificFriendNumber
                from CDWSNDFITSSO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OceanType specificOceanType = (OceanType)reader.GetByte(0);
                        int specificFriendNumber = reader.GetInt32(1);
                        requirement = new CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement(requirementID, specificOceanType, specificFriendNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToGetSpecificItemQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificItemID
                from GSI_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificItemID = reader.GetInt32(0);
                        requirement = new GetSpecificItemQuestRequirement(requirementID, specificItemID);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToCloseDealWithOutlanderQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT QuestRequirementID
                from CDWO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        requirement = new CloseDealWithOutlanderQuestRequirement(requirementID);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToCollectSpecificNumberBelongingGroupMaterialQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificMaterialNumber
                from CSNBGM_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificMaterialNumber = reader.GetInt32(0);
                        requirement = new CollectSpecificNumberBelongingGroupMaterialQuestRequirement(requirementID, specificMaterialNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToSynthesizeSpecificScoreMaterialQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificMaterialScore
                from SSSM_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificMaterialScore = reader.GetInt32(0);
                        requirement = new SynthesizeSpecificScoreMaterialQuestRequirement(requirementID, specificMaterialScore);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToScanSpecificQR_CodeQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT QR_CodeString
                from SSQRC_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string QR_CodeString = reader.GetString(0);
                        requirement = new ScanSpecificQR_CodeQuestRequirement(requirementID, QR_CodeString);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToHaveSpecificNumberFriendQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificFriendNumber
                from HSNF_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificFriendNumber = reader.GetInt32(0);
                        requirement = new HaveSpecificNumberFriendQuestRequirement(requirementID, specificFriendNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificSuccessfulNumber
                from SSSNOT_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificSuccessfulNumber = reader.GetInt32(0);
                        requirement = new SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement(requirementID, specificSuccessfulNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToHaveSpecificNumberKindMaterialQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificKindNumber
                from HSNKM_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificKindNumber = reader.GetInt32(0);
                        requirement = new HaveSpecificNumberKindMaterialQuestRequirement(requirementID, specificKindNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToAddSpecificNumberDecorationToVesselQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificDecorationNumber
                from ASNDTV_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificDecorationNumber = reader.GetInt32(0);
                        requirement = new AddSpecificNumberDecorationToVesselQuestRequirement(requirementID, specificDecorationNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRequirementToHaveSpecificNumberDecorationOnVesselQuestRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificDecorationNumber
                from HSNDOV_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int specificDecorationNumber = reader.GetInt32(0);
                        requirement = new HaveSpecificNumberDecorationOnVesselQuestRequirement(requirementID, specificDecorationNumber);
                        return true;
                    }
                    else
                    {
                        requirement = null;
                        return false;
                    }
                }
            }
        }
        #endregion

        #region Specialize QuestReward
        protected override bool SpecializeQuestRewardToGiveSpecificNumberSpecificItemQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT ItemCount, ItemID
                from givespecificnumberspecificitemquestrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int itemCount = reader.GetInt32(0);
                        int itemID = reader.GetInt32(1);

                        reward = new GiveSpecificNumberSpecificItemQuestReward(rewardID, itemCount, itemID);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRewardToUnlockSpecificBlueprintQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT BlueprintID
                from unlockspecificblueprintquestrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int blueprintID = reader.GetInt32(0);

                        reward = new UnlockSpecificBlueprintQuestReward(rewardID, blueprintID);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRewardToAcceptSpecificQuestQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT AcceptedQuesiID
                from acceptspecificquestquestrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int acceptedQuesiID = reader.GetInt32(0);

                        reward = new AcceptSpecificQuestQuestReward(rewardID, acceptedQuesiID);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreRandomMaterialQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT MaterialCount, MaterialScore
                from gsnssrm_questrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialCount = reader.GetInt32(0);
                        int materialScore = reader.GetInt32(1);

                        reward = new GiveSpecificNumberSpecificScoreRandomMaterialQuestReward(rewardID, materialCount, materialScore);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT MaterialCount, MaterialScore. GroupType
                from gsnsssgrm_questrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialCount = reader.GetInt32(0);
                        int materialScore = reader.GetInt32(1);
                        GroupType groupType = (GroupType)reader.GetByte(2);

                        reward = new GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward(rewardID, materialCount, materialScore, groupType);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        protected override bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT MaterialCount, MaterialScore
                from gsnssbgrm_questrewardcollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialCount = reader.GetInt32(0);
                        int materialScore = reader.GetInt32(1);

                        reward = new GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward(rewardID, materialCount, materialScore);
                        return true;
                    }
                    else
                    {
                        reward = null;
                        return false;
                    }
                }
            }
        }
        #endregion
    }
}
