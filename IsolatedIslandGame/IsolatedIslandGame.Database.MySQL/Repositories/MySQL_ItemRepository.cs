using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_ItemRepository : ItemRepository
    {
        public override bool Create(string itemName, string description, out Item item)
        {
            string sqlString = @"INSERT INTO ItemCollection 
                (ItemName,Description) VALUES (@itemName,@description) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("itemName", itemName);
                command.Parameters.AddWithValue("description", description);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int itemID = reader.GetInt32(0);
                        item = new Item(itemID, itemName, description);
                        return true;
                    }
                    else
                    {
                        item = null;
                        return false;
                    }
                }
            }
        }
        public override bool CreateMaterial(string itemName, string description, int score, out Material material)
        {
            Item item;
            if(Create(itemName, description, out item))
            {
                string sqlString = @"INSERT INTO MaterialCollection 
                (ItemID,Score) VALUES (@itemID,@score) ;
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("itemID", item.ItemID);
                    command.Parameters.AddWithValue("score", score);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int materialID = reader.GetInt32(0);
                            material = new Material(item.ItemID, item.ItemName, item.Description, materialID, score);
                            return true;
                        }
                        else
                        {
                            material = null;
                            return false;
                        }
                    }
                }
            }
            else
            {
                material = null;
                return false;
            }
        }
        public override bool Read(int itemID, out Item item)
        {
            int materialID = 0;
            int score = 0;
            string sqlString = @"SELECT  
                MaterialID, Score
                from MaterialCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@itemID", itemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        materialID = reader.GetInt32(0);
                        score = reader.GetInt32(1);
                    }
                }
            }
            sqlString = @"SELECT  
                ItemName, Description
                from ItemCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
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
                            item = new Item(itemID, itemName, description);
                        }
                        else
                        {
                            item = new Material(itemID, itemName, description, materialID, score);
                        }
                        return true;
                    }
                    else
                    {
                        item = null;
                        return false;
                    }
                }
            }
        }
        public override void Update(Item item)
        {
            string sqlString = @"UPDATE ItemCollection SET 
                ItemName = @itemName, Description = @description
                WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
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
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
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
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
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
                MaterialID, ItemID, Score
                from MaterialCollection;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int materialID = reader.GetInt32(0);
                        int itemID = reader.GetInt32(1);
                        int score = reader.GetInt32(2);
                        if (itemDictionary.ContainsKey(itemID))
                        {
                            Item item = itemDictionary[itemID];
                            itemDictionary[itemID] = new Material(item.ItemID, item.ItemName, item.Description, materialID, score);
                        }
                    }
                }
            }
            return itemDictionary.Values.ToList();
        }
    }
}
