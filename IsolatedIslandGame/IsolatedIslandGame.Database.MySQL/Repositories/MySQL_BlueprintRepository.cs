using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_BlueprintRepository : BlueprintRepository
    {
        public override Blueprint Create(bool isOrderless, bool isBlueprintRequired, Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products)
        {
            int blueprintID = 0;
            string sqlString = @"INSERT INTO BlueprintCollection 
                (BlueprintID,IsOrderless,IsBlueprintRequired) VALUES (NULL,@isOrderless,@isBlueprintRequired) ;
                SELECT LAST_INSERT_ID();";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("isOrderless", isOrderless);
                command.Parameters.AddWithValue("isBlueprintRequired", isBlueprintRequired);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        blueprintID = reader.GetInt32(0);
                    }
                }
            }
            for(int i = 0; i < requirements.Length; i++)
            {
                CreateRequirement(blueprintID, requirements[i]);
            }
            for (int i = 0; i < products.Length; i++)
            {
                CreateProduct(blueprintID, products[i]);
            }
            return new Blueprint(blueprintID, isOrderless, isBlueprintRequired, requirements, products);
        }
        public override void Delete(int blueprintID)
        {
            string sqlString = @"DELETE FROM BlueprintCollection 
                WHERE BlueprintID = @blueprintID;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintRepository Delete Blueprint Error BlueprintID: {0}", blueprintID);
                }
            }
        }
        public override List<Blueprint> ListAll()
        {
            List<BlueprintInfo> blueprintInfos = new List<BlueprintInfo>();
            string sqlString = @"SELECT BlueprintID, IsOrderless, IsBlueprintRequired from BlueprintCollection ;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int blueprintID = reader.GetInt32(0);
                        bool isOrderless = reader.GetBoolean(1);
                        bool isBlueprintRequired = reader.GetBoolean(2);
                        blueprintInfos.Add(new BlueprintInfo
                        {
                            blueprintID = blueprintID,
                            isOrderless = isOrderless,
                            isBlueprintRequired = isBlueprintRequired
                        });
                    }
                }
            }
            List<Blueprint> blueprints = new List<Blueprint>();
            foreach (BlueprintInfo blueprintInfo in blueprintInfos)
            {
                Blueprint.ElementInfo[] requirements = ListRequirementsOfBlueprint(blueprintInfo.blueprintID).ToArray();
                Blueprint.ElementInfo[] products = ListProductsOfBlueprint(blueprintInfo.blueprintID).ToArray();
                blueprints.Add(new Blueprint(blueprintInfo.blueprintID, blueprintInfo.isOrderless, blueprintInfo.isBlueprintRequired, requirements, products));
            }
            return blueprints;
        }

        public override Blueprint.ElementInfo CreateRequirement(int blueprintID, Blueprint.ElementInfo requirement)
        {
            string sqlString = @"INSERT INTO BlueprintRequirementCollection 
                (BlueprintID,ItemID,ItemCount,PositionIndex) VALUES (@blueprintID,@itemID,@itemCount,@positionIndex) ;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
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

        public override void DeleteRequirement(int blueprintID, Blueprint.ElementInfo requirement)
        {
            string sqlString = @"DELETE FROM BlueprintRequirementCollection 
                WHERE BlueprintID = @blueprintID AND ItemID = @itemID AND ItemCount = @itemCount AND PositionIndex = @positionIndex;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
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

        public override List<Blueprint.ElementInfo> ListRequirementsOfBlueprint(int blueprintID)
        {
            List<Blueprint.ElementInfo> requirements = new List<Blueprint.ElementInfo>();
            string sqlString = @"SELECT  
                ItemID, ItemCount, PositionIndex
                from BlueprintRequirementCollection 
                WHERE BlueprintID = @blueprintID;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
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

        public override Blueprint.ElementInfo CreateProduct(int blueprintID, Blueprint.ElementInfo product)
        {
            string sqlString = @"INSERT INTO BlueprintProductCollection 
                (BlueprintID,ItemID,ItemCount,PositionIndex) VALUES (@blueprintID,@itemID,@itemCount,@positionIndex) ;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                command.Parameters.AddWithValue("itemID", product.itemID);
                command.Parameters.AddWithValue("itemCount", product.itemCount);
                command.Parameters.AddWithValue("positionIndex", product.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintProductRequirement Create BlueprintProduct Error BlueprintID: {0}, RequirementInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, product.itemID, product.itemCount, product.positionIndex);
                }
            }
            return product;
        }

        public override void DeleteProduct(int blueprintID, Blueprint.ElementInfo product)
        {
            string sqlString = @"DELETE FROM BlueprintProductCollection 
                WHERE BlueprintID = @blueprintID AND ItemID = @itemID AND ItemCount = @itemCount AND PositionIndex = @positionIndex;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("blueprintID", blueprintID);
                command.Parameters.AddWithValue("itemID", product.itemID);
                command.Parameters.AddWithValue("itemCount", product.itemCount);
                command.Parameters.AddWithValue("positionIndex", product.positionIndex);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintProductRepository Delete BlueprintProduct Error BlueprintID: {0}, ProductInfo: ItemID:{1}, ItemCount: {2}, PositionIndex: {3}", blueprintID, product.itemID, product.itemCount, product.positionIndex);
                }
            }
        }

        public override List<Blueprint.ElementInfo> ListProductsOfBlueprint(int blueprintID)
        {
            List<Blueprint.ElementInfo> products = new List<Blueprint.ElementInfo>();
            string sqlString = @"SELECT  
                ItemID, ItemCount, PositionIndex
                from BlueprintProductCollection 
                WHERE BlueprintID = @blueprintID;";
            lock(DatabaseService.ConnectionList.SettingDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.Connection as MySqlConnection))
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
