using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_DecorationRepository : DecorationRepository
    {
        public override bool Create(int vesselID, int materialItemID, float positionX, float positionY, float positionZ, float eulerAngleX, float eulerAngleY, float eulerAngleZ, out Decoration decoration)
        {
            string sqlString = @"INSERT INTO DecorationCollection 
                (VesselID,MaterialItemID,PositionX,PositionY,PositionZ,EulerAngleX,EulerAngleY,EulerAngleZ) VALUES (@vesselID,@materialItemID,@positionX,@positionY,@positionZ,@eulerAngleX,@eulerAngleY,@eulerAngleZ) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("vesselID", vesselID);
                command.Parameters.AddWithValue("materialItemID", materialItemID);
                command.Parameters.AddWithValue("positionX", positionX);
                command.Parameters.AddWithValue("positionY", positionY);
                command.Parameters.AddWithValue("positionZ", positionZ);
                command.Parameters.AddWithValue("eulerAngleX", eulerAngleX);
                command.Parameters.AddWithValue("eulerAngleY", eulerAngleY);
                command.Parameters.AddWithValue("eulerAngleZ", eulerAngleZ);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int decorationID = reader.GetInt32(0);
                        Item material;
                        if (ItemManager.Instance.FindItem(materialItemID, out material))
                        {
                            decoration = new Decoration(decorationID, material as Material, positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ);
                            return true;
                        }
                        else
                        {
                            decoration = null;
                            return false;
                        }
                    }
                    else
                    {
                        decoration = null;
                        return false;
                    }
                }
            }
        }
        public override bool Read(int decorationID, out Decoration decoration)
        {
            string sqlString = @"SELECT  
                MaterialItemID, PositionX, PositionY, PositionZ, EulerAngleX, EulerAngleY, EulerAngleZ 
                from DecorationCollection WHERE DecorationID = @decorationID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("decorationID", decorationID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int materialItemID = reader.GetInt32(0);
                        float positionX = reader.GetFloat(1);
                        float positionY = reader.GetFloat(2);
                        float positionZ = reader.GetFloat(3);
                        float eulerAngleX = reader.GetFloat(4);
                        float eulerAngleY = reader.GetFloat(5);
                        float eulerAngleZ = reader.GetFloat(6);
                        Item material;
                        if (ItemManager.Instance.FindItem(materialItemID, out material))
                        {
                            decoration = new Decoration(decorationID, material as Material, positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ);
                            return true;
                        }
                        else
                        {
                            decoration = null;
                            return false;
                        }
                    }
                    else
                    {
                        decoration = null;
                        return false;
                    }
                }
            }
        }  
        public override void Update(Decoration decoration, int vesselID)
        {
            string sqlString = @"UPDATE DecorationCollection SET 
                VesselID = @vesselID, MaterialItemID = @materialItemID, PositionX = @positionX, PositionY = @positionY, PositionZ = @positionZ, EulerAngleX = @eulerAngleX, EulerAngleY = @eulerAngleY, EulerAngleZ  = @eulerAngleZ
                WHERE DecorationID = @decorationID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("vesselID", vesselID);
                command.Parameters.AddWithValue("materialItemID", decoration.Material.ItemID);
                command.Parameters.AddWithValue("positionX", decoration.PositionX);
                command.Parameters.AddWithValue("positionY", decoration.PositionY);
                command.Parameters.AddWithValue("positionZ", decoration.PositionZ);
                command.Parameters.AddWithValue("eulerAngleX", decoration.RotationEulerAngleX);
                command.Parameters.AddWithValue("eulerAngleY", decoration.RotationEulerAngleY);
                command.Parameters.AddWithValue("eulerAngleZ", decoration.RotationEulerAngleZ);
                command.Parameters.AddWithValue("decorationID", decoration.DecorationID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_DecorationRepository Save Decoration Error DecorationID: {0}", decoration.DecorationID);
                }
            }
        }
        public override void Delete(int decorationID)
        {
            string sqlString = @"DELETE FROM DecorationCollection 
                WHERE DecorationID = @decorationID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("decorationID", decorationID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_DecorationRepository Delete Decoration Error DecorationID: {0}", decorationID);
                }
            }
        }
        public override List<Decoration> ListOfVessel(int vesselID)
        {
            List<Decoration> decorations = new List<Decoration>();
            string sqlString = @"SELECT  
                DecorationID, MaterialItemID, PositionX, PositionY, PositionZ, EulerAngleX, EulerAngleY, EulerAngleZ
                from DecorationCollection 
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("vesselID", vesselID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int decorationID = reader.GetInt32(0);
                        int materialItemID = reader.GetInt32(1);
                        float positionX = reader.GetFloat(2);
                        float positionY = reader.GetFloat(3);
                        float positionZ = reader.GetFloat(4);
                        float eulerAngleX = reader.GetFloat(5);
                        float eulerAngleY = reader.GetFloat(6);
                        float eulerAngleZ = reader.GetFloat(7);
                        Item material;
                        if (ItemManager.Instance.FindItem(materialItemID, out material))
                        {
                            decorations.Add(new Decoration(decorationID, material as Material, positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ));
                        }
                    }
                }
            }
            return decorations;
        }
    }
}
