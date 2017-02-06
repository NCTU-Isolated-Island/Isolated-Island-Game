﻿using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
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
            List<int> requirementIDs = new List<int>();
            string sqlString = @"SELECT QuestRequirementID from QuestRequirementCollection 
                WHERE QuestID = @questID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questID", questID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRequirementID = reader.GetInt32(0);
                        requirementIDs.Add(questRequirementID);
                    }
                }
            }
            List<QuestRequirement> requirements = new List<QuestRequirement>();
            foreach(int questRequirementID in requirementIDs)
            {
                QuestRequirement requirement;
                if (SpecializeRequirementToSendMessageToDifferentOnlineFriendRequirement(questRequirementID, out requirement))
                {
                    requirements.Add(requirement);
                }
            }
            return requirements;
        }

        protected override List<QuestReward> ListRewardsOfQuest(int questID)
        {
            List<int> rewardIDs = new List<int>();
            string sqlString = @"SELECT QuestRewardID from QuestRewardCollection 
                WHERE QuestID = @questID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("questID", questID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questRewardID = reader.GetInt32(0);
                        rewardIDs.Add(questRewardID);
                    }
                }
            }
            List<QuestReward> rewards = new List<QuestReward>();
            foreach (int questRewardID in rewardIDs)
            {
                QuestReward reward;
                if (SpecializeRewardToGiveItemReward(questRewardID, out reward))
                {
                    rewards.Add(reward);
                }
            }
            return rewards;
        }

        #region Specialize QuestRequirement
        protected override bool SpecializeRequirementToSendMessageToDifferentOnlineFriendRequirement(int requirementID, out QuestRequirement requirement)
        {
            string sqlString = @"SELECT RequiredOnlinedFriendNumber
                from SendMessageToDifferentOnlineFriendQuestRequirementCollection WHERE QuestRequirementID = @requirementID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requirementID", requirementID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int requiredOnlinedFriendNumber = reader.GetInt32(0);
                        requirement = new SendMessageToDifferentOnlineFriendQuestRequirement(requirementID, requiredOnlinedFriendNumber);
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
                        Item item;
                        if(ItemManager.Instance.FindItem(itemID, out item))
                        {
                            reward = new GiveItemQuestReward(rewardID, item, itemCount);
                            return true;
                        }
                        else
                        {
                            reward = null;
                            return false;
                        }
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