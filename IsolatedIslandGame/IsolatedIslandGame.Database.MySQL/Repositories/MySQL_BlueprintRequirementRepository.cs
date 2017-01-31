using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_BlueprintRequirementRepository : BlueprintRequirementRepository
    {
        public override Blueprint.ElementInfo Create(int blueprintID, Blueprint.ElementInfo requirement)
        {
            string sqlString = @"INSERT INTO BlueprintRequirementCollection 
                (BlueprintID,ItemID,ItemCount,PositionIndex) VALUES (@blueprintID,@itemID,@itemCount,@positionIndex) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                command.Parameters.AddWithValue("itemID", requirement.itemID);
                command.Parameters.AddWithValue("itemCount", requirement.itemCount);
                command.Parameters.AddWithValue("positionIndex", requirement.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintRequirement Create BlueprintRequirement Error BlueprintID: {0}, RequirementInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, requirement.itemID, requirement.itemCount, requirement.positionIndex);
                }
            }
            return requirement;
        }

        public override void Delete(int blueprintID, Blueprint.ElementInfo requirement)
        {
            string sqlString = @"DELETE FROM BlueprintRequirementCollection 
                WHERE BlueprintID = @blueprintID AND ItemID = @itemID AND ItemCount = @itemCount AND PositionIndex = @positionIndex;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                command.Parameters.AddWithValue("itemID", requirement.itemID);
                command.Parameters.AddWithValue("itemCount", requirement.itemCount);
                command.Parameters.AddWithValue("positionIndex", requirement.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintRequirementRepository Delete BlueprintRequirement Error BlueprintID: {0}, RequirementInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, requirement.itemID, requirement.itemCount, requirement.positionIndex);
                }
            }
        }

        public override List<Blueprint.ElementInfo> ListOfBlueprint(int blueprintID)
        {
            List<Blueprint.ElementInfo> requirements = new List<Blueprint.ElementInfo>();
            string sqlString = @"SELECT  
                ItemID, ItemCount, PositionIndex
                from BlueprintRequirementCollection 
                WHERE BlueprintID = @blueprintID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        int itemCount = reader.GetInt32(1);
                        int positionIndex = reader.GetInt32(2);
                        requirements.Add(new Blueprint.ElementInfo { itemID = itemID, itemCount = itemCount, positionIndex = positionIndex });
                    }
                }
            }
            return requirements;
        }
    }
}
