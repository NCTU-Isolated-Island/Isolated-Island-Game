using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories
{
    class MySQL_VesselRepository : VesselRepository
    {
        public override Vessel Create(int ownerPlayerID, string name)
        {
            string sqlString = @"INSERT INTO VesselCollection 
                (OwnerPlayerID,Name) VALUES (@ownerPlayerID,@name) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("ownerPlayerID", ownerPlayerID);
                command.Parameters.AddWithValue("name", name);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        return new Vessel(vesselID, ownerPlayerID, name, 0, 0, 0);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public override Vessel Read(int vesselID)
        {
            Vessel vessel = null;
            string sqlString = @"SELECT  
                OwnerPlayerID, Name, LocationX, LocationZ, EulerAngleY
                from VesselCollection WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@vesselID", vesselID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int ownerPlayerID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        float locationX = reader.GetFloat(2);
                        float locationZ = reader.GetFloat(3);
                        float eulerAngleY = reader.GetFloat(4);
                        vessel = new Vessel(vesselID, ownerPlayerID, name, locationX, locationZ, eulerAngleY);
                    }
                }
            }
            if (vessel != null)
            {
                var decorations = DatabaseService.RepositoryList.DecorationRepository.ListOfVessel(vessel.VesselID);
                foreach (var decoration in decorations)
                {
                    vessel.AddDecoration(decoration);
                }
            }
            return vessel;
        }
        public override Vessel ReadByOwnerPlayerID(int ownerPlayerID)
        {
            Vessel vessel = null;
            string sqlString = @"SELECT  
                VesselID, Name, LocationX, LocationZ, EulerAngleY
                from VesselCollection WHERE OwnerPlayerID = @ownerPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@ownerPlayerID", ownerPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        float locationX = reader.GetFloat(2);
                        float locationZ = reader.GetFloat(3);
                        float eulerAngleY = reader.GetFloat(4);
                        vessel = new Vessel(vesselID, ownerPlayerID, name, locationX, locationZ, eulerAngleY);
                    }
                }
            }
            if (vessel != null)
            {
                var decorations = DatabaseService.RepositoryList.DecorationRepository.ListOfVessel(vessel.VesselID);
                foreach (var decoration in decorations)
                {
                    vessel.AddDecoration(decoration);
                }
            }
            return vessel;
        }
        public override void Update(Vessel vessel)
        {
            string sqlString = @"UPDATE VesselCollection SET 
                Name = @name, LocationX = @locationX, LocationZ = @locationZ, EulerAngleY = @eulerAngleY
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@name", vessel.Name);
                command.Parameters.AddWithValue("@locationX", vessel.LocationX);
                command.Parameters.AddWithValue("@locationZ", vessel.LocationZ);
                command.Parameters.AddWithValue("@eulerAngleY", vessel.RotationEulerAngleY);
                command.Parameters.AddWithValue("@vesselID", vessel.VesselID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_VesselRepository Save Vessel Error VesselID: {0}", vessel.VesselID);
                }
            }
            foreach (var decoration in vessel.Decorations)
            {
                DatabaseService.RepositoryList.DecorationRepository.Update(decoration, vessel.VesselID);
            }
        }
        public override void Delete(int vesselID)
        {
            string sqlString = @"DELETE FROM VesselCollection 
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@vesselID", vesselID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_VesselRepository Delete Vessel Error VesselID: {0}", vesselID);
                }
            }
        }
    }
}
