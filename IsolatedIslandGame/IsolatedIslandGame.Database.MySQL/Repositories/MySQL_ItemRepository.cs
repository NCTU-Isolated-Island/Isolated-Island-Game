using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_ItemRepository : ItemRepository
    {
        public override bool Read(int itemID, out Item item)
        {
            int materialID = 0;
            int score = 0;
            GroupType groupType = GroupType.No;
            int level = 0;
            string sqlString = @"SELECT  
                MaterialID, Score, GroupType, Level
                from MaterialCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("itemID", itemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        materialID = reader.GetInt32(0);
                        score = reader.GetInt32(1);
                        groupType = (GroupType)reader.GetByte(2);
                        level = reader.GetInt32(3);
                    }
                }
            }
            sqlString = @"SELECT  
                ItemName, Description
                from ItemCollection WHERE ItemID = @itemID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("itemID", itemID);
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
                            item = new Material(itemID, itemName, description, materialID, score, groupType, level);
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
                MaterialID, ItemID, Score, GroupType, Level
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
                        GroupType groupType = (GroupType)reader.GetByte(3);
                        int level = reader.GetInt32(4);
                        if (itemDictionary.ContainsKey(itemID))
                        {
                            Item item = itemDictionary[itemID];
                            itemDictionary[itemID] = new Material(item.ItemID, item.ItemName, item.Description, materialID, score, groupType, level);
                        }
                    }
                }
            }
            return itemDictionary.Values.ToList();
        }
    }
}
