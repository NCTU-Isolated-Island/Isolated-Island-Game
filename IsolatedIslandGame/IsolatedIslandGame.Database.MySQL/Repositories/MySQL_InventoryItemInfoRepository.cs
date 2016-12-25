using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_InventoryItemInfoRepository : InventoryItemInfoRepository
    {
        public override bool Create(int inventoryID, int itemID, int itemCount, int positionIndex, out InventoryItemInfo info)
        {
            string sqlString = @"INSERT INTO InventoryItemInfoCollection 
                (InventoryID,ItemID,ItemCount,PositionIndex) VALUES (@inventoryID,@itemID,@itemCount,@positionIndex) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("inventoryID", inventoryID);
                command.Parameters.AddWithValue("itemID", itemID);
                command.Parameters.AddWithValue("itemCount", itemCount);
                command.Parameters.AddWithValue("positionIndex", positionIndex);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int inventoryItemInfoID = reader.GetInt32(0);
                        Item item;
                        if(ItemManager.Instance.FindItem(itemID, out item))
                        {
                            info = new InventoryItemInfo(inventoryItemInfoID, item, itemCount, positionIndex);
                            return true;
                        }
                        else
                        {
                            info = null;
                            return false;
                        }
                    }
                    else
                    {
                        info = null;
                        return false;
                    }
                }
            }
        }
        public override bool Read(int inventoryItemInfoID, out InventoryItemInfo info)
        {
            string sqlString = @"SELECT  
                InventoryID, ItemID, ItemCount, PositionIndex
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
                        Item item;
                        if (ItemManager.Instance.FindItem(itemID, out item))
                        {
                            info = new InventoryItemInfo(inventoryItemInfoID, item, itemCount, positionIndex);
                            return true;
                        }
                        else
                        {
                            info = null;
                            return false;
                        }
                    }
                    else
                    {
                        info = null;
                        return false;
                    }
                }
            }
        }
        public override void Update(InventoryItemInfo info, int inventoryID)
        {
            string sqlString = @"UPDATE InventoryItemInfoCollection SET 
                InventoryID = @inventoryID, ItemID = @itemID, ItemCount = @itemCount, PositionIndex = @positionIndex
                WHERE InventoryItemInfoID = @inventoryItemInfoID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inventoryID", inventoryID);
                command.Parameters.AddWithValue("@itemID", info.Item.ItemID);
                command.Parameters.AddWithValue("@itemCount", info.Count);
                command.Parameters.AddWithValue("@positionIndex", info.PositionIndex);
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
                InventoryItemInfoID, ItemID, ItemCount, PositionIndex
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
                        Item item;
                        if (ItemManager.Instance.FindItem(itemID, out item))
                        {
                            items.Add(new InventoryItemInfo(inventoryItemInfoID, item, itemCount, positionIndex));
                        }
                    }
                }
            }
            return items;
        }
    }
}
