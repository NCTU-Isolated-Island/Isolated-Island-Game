using IsolatedIslandGame.Database.Repositories;
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
                    case QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameSpecificOcean:
                        if (SpecializeRequirementToSendMessageToDifferentOnlineFriendTheSameSpecificOceanRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CloseDealWithDifferentFriendInTheSameSpecificOcean:
                        if (SpecializeRequirementToCloseDealWithDifferentFriendInTheSameSpecificOceanRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.ScanQR_Code:
                        if (SpecializeRequirementToScanQR_CodeRequirement(info.questRequirementID, out requirement))
                        {
                            requirements.Add(requirement);
                        }
                        break;
                    case QuestRequirementType.CumulativeLogin:
                        if (SpecializeRequirementToCumulativeLoginRequirement(info.questRequirementID, out requirement))
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
                    case QuestRewardType.GiveItem:
                        if (SpecializeRewardToGiveItemReward(info.questRewardID, out reward))
                        {
                            rewards.Add(reward);
                        }
                        break;
                    case QuestRewardType.UnlockBlueprint:
                        if (SpecializeRewardToUnlockBlueprintReward(info.questRewardID, out reward))
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
        protected override bool SpecializeRequirementToSendMessageToDifferentOnlineFriendTheSameSpecificOceanRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanType, RequiredFriendNumber
                from SMTDOFITSSO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OceanType specificOceanType = (OceanType)reader.GetByte(0);
                        int requiredFriendNumber = reader.GetInt32(1);
                        requirement = new SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(requirementID, specificOceanType, requiredFriendNumber);
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
        protected override bool SpecializeRequirementToCloseDealWithDifferentFriendInTheSameSpecificOceanRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT SpecificOceanType, RequiredFriendNumber
                from CDWDFITSSO_QuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OceanType specificOceanType = (OceanType)reader.GetByte(0);
                        int requiredFriendNumber = reader.GetInt32(1);
                        requirement = new CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement(requirementID, specificOceanType, requiredFriendNumber);
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
        protected override bool SpecializeRequirementToScanQR_CodeRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT QR_CodeString
                from ScanQR_CodeQuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string qrCodeString = reader.GetString(0);
                        requirement = new ScanQR_CodeQuestRequirement(requirementID, qrCodeString);
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
        protected override bool SpecializeRequirementToCumulativeLoginRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT CumulativeLoginCount
                from CumulativeLoginQuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int cumulativeLoginCount = reader.GetInt32(0);
                        requirement = new CumulativeLoginQuestRequirement(requirementID, cumulativeLoginCount);
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
        protected override bool SpecializeRewardToGiveItemReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT ItemID, ItemCount
                from GiveItemQuestRewardCollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        int itemCount = reader.GetInt32(1);

                        reward = new GiveItemQuestReward(rewardID, itemID, itemCount);
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
        protected override bool SpecializeRewardToUnlockBlueprintReward(int rewardID, out QuestReward reward)
        {
            string sqlString = @"SELECT BlueprintID
                from UnlockBlueprintQuestRewardCollection WHERE QuestRewardID = @rewardID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("rewardID", rewardID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int blueprintID = reader.GetInt32(0);

                        reward = new UnlockBlueprintQuestReward(rewardID, blueprintID);
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
