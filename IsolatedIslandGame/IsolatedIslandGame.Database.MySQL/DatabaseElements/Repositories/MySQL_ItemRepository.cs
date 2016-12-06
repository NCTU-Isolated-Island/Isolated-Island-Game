using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories
{
    class MySQL_ItemRepository : ItemRepository
    {
        public override Item Create(string itemName, string description)
        {
            string sqlString = @"INSERT INTO ItemCollection 
                (ItemName,Description) VALUES (@itemName,@description) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("itemName", itemName);
                command.Parameters.AddWithValue("description", description);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        return new Item(itemID, itemName, description);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override Material CreateMaterial(string itemName, string description)
        {
            Item item = Create(itemName, description);
            string sqlString = @"INSERT INTO MaterialCollection 
                (ItemID) VALUES (@itemID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("itemID", item.ItemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialID = reader.GetInt32(0);
                        return new Material(item.ItemID, item.ItemName, item.Description, materialID);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override Item Read(int itemID)
        {
            int materialID = 0;
            string sqlString = @"SELECT  
                MaterialID
                from MaterialCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@itemID", itemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        materialID = reader.GetInt32(0);
                    }
                }
            }
            sqlString = @"SELECT  
                ItemName, Description
                from ItemCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@itemID", itemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string itemName = reader.GetString(0);
                        string description = reader.GetString(1);
                        if(materialID == 0)
                        {
                            return new Item(itemID, itemName, description);
                        }
                        else
                        {
                            return new Material(itemID, itemName, description, materialID);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override void Update(Item item)
        {
            string sqlString = @"UPDATE ItemCollection SET 
                ItemName = @itemName, Description = @description
                WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@itemName", item.ItemName);
                command.Parameters.AddWithValue("@description", item.Description);
                command.Parameters.AddWithValue("@itemID", item.ItemID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_ItemRepository Save Item Error ItemID: {0}", item.ItemID);
                }
            }
        }
        public override void Delete(int itemID)
        {
            string sqlString = @"DELETE FROM ItemCollection 
                WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@itemID", itemID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_ItemRepository Delete Item Error ItemID: {0}", itemID);
                }
            }
        }
        public override List<Item> ListAll()
        {
            Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();
            string sqlString = @"SELECT  
                ItemID,ItemName, Description
                from ItemCollection;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        string itemName = reader.GetString(1);
                        string description = reader.GetString(2);
                        itemDictionary.Add(itemID, new Item(itemID, itemName, description));
                    }
                }
            }
            sqlString = @"SELECT  
                MaterialID, ItemID
                from MaterialCollection;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ItemDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int materialID = reader.GetInt32(0);
                        int itemID = reader.GetInt32(1);
                        if(itemDictionary.ContainsKey(itemID))
                        {
                            Item item = itemDictionary[itemID];
                            itemDictionary[itemID] = new Material(item.ItemID, item.ItemName, item.Description, materialID);
                        }
                    }
                }
            }
            return itemDictionary.Values.ToList();
        }
    }
}
