using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories
{
    class MySQL_BlueprintProductRepository : BlueprintProductRepository
    {
        public override Blueprint.ElementInfo Create(int blueprintID, Blueprint.ElementInfo product)
        {
            string sqlString = @"INSERT INTO BlueprintProductCollection 
                (BlueprintID,ItemID,ItemCount,PositionIndex) VALUES (@blueprintID,@itemID,@itemCount,@positionIndex) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@blueprintID", blueprintID);
                command.Parameters.AddWithValue("@itemID", product.itemID);
                command.Parameters.AddWithValue("@itemCount", product.itemCount);
                command.Parameters.AddWithValue("@positionIndex", product.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintProductRequirement Create BlueprintProduct Error BlueprintID: {0}, RequirementInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, product.itemID, product.itemCount, product.positionIndex);
                }
            }
            return product;
        }

        public override void Delete(int blueprintID, Blueprint.ElementInfo product)
        {
            string sqlString = @"DELETE FROM BlueprintProductCollection 
                WHERE BlueprintID = @blueprintID AND ItemID = @itemID AND ItemCount = @itemCount AND PositionIndex = @positionIndex;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@blueprintID", blueprintID);
                command.Parameters.AddWithValue("@itemID", product.itemID);
                command.Parameters.AddWithValue("@itemCount", product.itemCount);
                command.Parameters.AddWithValue("@positionIndex", product.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintProductRepository Delete BlueprintProduct Error BlueprintID: {0}, ProductInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, product.itemID, product.itemCount, product.positionIndex);
                }
            }
        }

        public override List<Blueprint.ElementInfo> ListOfBlueprint(int blueprintID)
        {
            List<Blueprint.ElementInfo> products = new List<Blueprint.ElementInfo>();
            string sqlString = @"SELECT  
                ItemID, ItemCount, PositionIndex
                from BlueprintProductCollection 
                WHERE BlueprintID = @blueprintID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@blueprintID", blueprintID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        int itemCount = reader.GetInt32(1);
                        int positionIndex = reader.GetInt32(2);
                        products.Add(new Blueprint.ElementInfo { itemID = itemID, itemCount = itemCount, positionIndex = positionIndex });
                    }
                }
            }
            return products;
        }
    }
}
