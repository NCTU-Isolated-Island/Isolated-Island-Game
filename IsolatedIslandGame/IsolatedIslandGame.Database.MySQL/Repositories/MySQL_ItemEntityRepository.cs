using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_ItemEntityRepository : ItemEntityRepository
    {
        public override bool Create(int itemID, float positionX, float positionZ, out ItemEntity itemEntity)
        {
            string sqlString = @"INSERT INTO ItemEntityCollection 
                (ItemID,PositionX,PositionZ) VALUES (@itemID,@positionX,@positionZ) ;
                SELECT LAST_INSERT_ID();";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("itemID", itemID);
                    command.Parameters.AddWithValue("positionX", positionX);
                    command.Parameters.AddWithValue("positionZ", positionZ);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int itemEntityID = reader.GetInt32(0);
                            itemEntity = new ItemEntity(itemEntityID, itemID, positionX, positionZ);
                            return true;
                        }
                        else
                        {
                            itemEntity = null;
                            return false;
                        }
                    }
                }
        }
        public override bool Read(int itemEntityID, out ItemEntity itemEntity)
        {
            string sqlString = @"SELECT  
                ItemID, PositionX, PositionZ
                from ItemEntityCollection WHERE ItemEntityID = @itemEntityID;";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("itemEntityID", itemEntityID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int itemID = reader.GetInt32(0);
                            float positionX = reader.GetFloat(1);
                            float positionZ = reader.GetFloat(2);

                            itemEntity = new ItemEntity(itemEntityID, itemID, positionX, positionZ);
                            return true;
                        }
                        else
                        {
                            itemEntity = null;
                            return false;
                        }
                    }
                }
        }
        public override void Delete(int itemEntityID)
        {
            string sqlString = @"DELETE FROM ItemEntityCollection 
                WHERE ItemEntityID = @itemEntityID;";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("itemEntityID", itemEntityID);
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        LogService.ErrorFormat("MySQL_ItemEntityRepository Delete InventoryItemInfo Error ItemEntityID: {0}", itemEntityID);
                    }
                }
        }
        public override List<ItemEntity> ListAll()
        {
            List<ItemEntity> itemEntities = new List<ItemEntity>();
            string sqlString = @"SELECT  
                ItemEntityID, ItemID, PositionX, PositionZ
                from ItemEntityCollection ;";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemEntityID = reader.GetInt32(0);
                            int itemID = reader.GetInt32(1);
                            float positionX = reader.GetFloat(2);
                            float positionZ = reader.GetFloat(3);

                            itemEntities.Add(new ItemEntity(itemEntityID, itemID, positionX, positionZ));
                        }
                    }
                }
            return itemEntities;
        }
    }
}
