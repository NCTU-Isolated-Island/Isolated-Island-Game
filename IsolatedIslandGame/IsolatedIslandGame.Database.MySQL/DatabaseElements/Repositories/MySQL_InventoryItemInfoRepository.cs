using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories
{
    class MySQL_InventoryItemInfoRepository : InventoryItemInfoRepository
    {
        public override InventoryItemInfo Create(int inventoryID, int itemID, int itemCount, int positionIndex, bool isUsing)
        {
            string sqlString = @"INSERT INTO InventoryItemInfoCollection 
                (InventoryID,ItemID,ItemCount,PositionIndex,IsUsing) VALUES (@inventoryID,@itemID,@itemCount,@positionIndex,@isUsing) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("inventoryID", inventoryID);
                command.Parameters.AddWithValue("itemID", itemID);
                command.Parameters.AddWithValue("itemCount", itemCount);
                command.Parameters.AddWithValue("positionIndex", positionIndex);
                command.Parameters.AddWithValue("isUsing", isUsing);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int inventoryItemInfoID = reader.GetInt32(0);
                        return new InventoryItemInfo(inventoryItemInfoID, ItemManager.Instance.FindItem(itemID), itemCount, positionIndex, isUsing);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override InventoryItemInfo Read(int inventoryItemInfoID)
        {
            string sqlString = @"SELECT  
                InventoryID, ItemID, ItemCount, PositionIndex, IsUsing
                from InventoryItemInfoCollection WHERE InventoryItemInfoID = @inventoryItemInfoID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inventoryItemInfoID", inventoryItemInfoID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int inventoryID = reader.GetInt32(0);
                        int itemID = reader.GetInt32(1);
                        int itemCount = reader.GetInt32(2);
                        int positionIndex = reader.GetInt32(3);
                        bool isUsing = reader.GetBoolean(4);
                        return new InventoryItemInfo(inventoryItemInfoID, ItemManager.Instance.FindItem(itemID), itemCount, positionIndex, isUsing);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override void Update(InventoryItemInfo info, int inventoryID)
        {
            string sqlString = @"UPDATE InventoryItemInfoCollection SET 
                InventoryID = @inventoryID, ItemID = @itemID, ItemCount = @itemCount, PositionIndex = @positionIndex, IsUsing = @isUsing
                WHERE InventoryItemInfoID = @inventoryItemInfoID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inventoryID", inventoryID);
                command.Parameters.AddWithValue("@itemID", info.Item.ItemID);
                command.Parameters.AddWithValue("@itemCount", info.Count);
                command.Parameters.AddWithValue("@positionIndex", info.PositionIndex);
                command.Parameters.AddWithValue("@isUsing", info.IsUsing);
                command.Parameters.AddWithValue("@inventoryItemInfoID", info.InventoryItemInfoID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_InventoryItemInfoRepository Save InventoryItemInfo Error InventoryItemInfoID: {0}", info.InventoryItemInfoID);
                }
            }
        }
        public override void Delete(int inventoryItemInfoID)
        {
            string sqlString = @"DELETE FROM InventoryItemInfoCollection 
                WHERE InventoryItemInfoID = @inventoryItemInfoID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inventoryItemInfoID", inventoryItemInfoID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_InventoryItemInfoRepository Delete InventoryItemInfo Error InventoryItemInfoID: {0}", inventoryItemInfoID);
                }
            }
        }

        public override List<InventoryItemInfo> ListOfInventory(int inventoryID)
        {
            List<InventoryItemInfo> items = new List<InventoryItemInfo>();
            string sqlString = @"SELECT  
                InventoryItemInfoID, ItemID, ItemCount, PositionIndex, IsUsing
                from InventoryItemInfoCollection 
                WHERE InventoryID = @inventoryID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inventoryID", inventoryID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int inventoryItemInfoID = reader.GetInt32(0);
                        int itemID = reader.GetInt32(1);
                        int itemCount = reader.GetInt32(2);
                        int positionIndex = reader.GetInt32(3);
                        bool isUsing = reader.GetBoolean(4);
                        items.Add(new InventoryItemInfo(inventoryItemInfoID, ItemManager.Instance.FindItem(itemID), itemCount, positionIndex, isUsing));
                    }
                }
            }
            return items;
        }
    }
}
